#include "global.h"
#include "common.h"
#include "sound.h"
#include "save.h"
#include "screen.h"

// BEGIN configure these in EA
extern u32 (*const TimerGetTime)(void);
extern u32 (*const CounterGetValue)(void);
extern const u32 counterMaxValue;
// END

int pow(int base, int exponent) {
    int result = 1;
    for (int i = 0; i < exponent; i++) {
        result *= base;
    }
    return result;
}

int log(int num, int base) {
    int result = 0;
    while (num >= base) {
        num /= base;
        result++;
    }
    return result;
}

extern u8 RupeeKeyDigits[];

void RenderDigits(u32 iconVramIndex, u32 count, u32 isTextYellow, u32 digits) {
    int iVar2;
    int iVar3;
    u8* puVar4;

    puVar4 = RupeeKeyDigits;
    if (isTextYellow == 0) {
        puVar4 -= 0x280;
    }
    iVar3 = (iconVramIndex & 0x3ff) * 0x20;
    iVar2 = iVar3 + 0x600c000;
    while (digits > 0) {
        int p = pow(10, --digits);
        int digit = count / p;
        count = count % p;
        DmaCopy32(3, puVar4 + digit * 0x40, iVar2, 0x10 * 4);
        iVar2 += 0x40;
    }
}

extern u16 gPaletteBuffer[];
const u16 walletColors[] = {
    0x0e06, 0x0f28, 0x5163, 0x7e65, 0x0caf, 0x10d7, 0x0caf, 0x10d7,
};

void DrawRupees(void) {
    u32 cVar1;
    u32 temp;
    u32 temp2;
    u16* row1;
    u16* row2;

    int digits = log(gWalletSizes[gSave.stats.walletType].size, 10) + 1;

    if ((gUnk_0200AF00.unk_1 & 0x40) != 0) {
        if (gUnk_0200AF00.unk_a != 0) {
            gUnk_0200AF00.unk_a = 0;
            row1 = &gBG0Buffer[0x25b - digits];
            row1[0] = 0;
            row1[1] = 0;
            row1[2] = 0;
            row1[3] = 0;
            row1[4] = 0;
            row1[5] = 0;
            row1[6] = 0;
            row2 = &gBG0Buffer[0x27b - digits];
            row2[0] = 0;
            row2[1] = 0;
            row2[2] = 0;
            row2[3] = 0;
            row2[4] = 0;
            row2[5] = 0;
            row2[6] = 0;
            gScreen.bg0.updated = 1;
        }
    } else {
        if (gUnk_0200AF00.unk_a == 0) {
            gUnk_0200AF00.unk_a = 2;
            row1 = &gBG0Buffer[0x25b - digits];
            row2 = &gBG0Buffer[0x27b - digits];
            row1[0] = temp2 = gWalletSizes[gSave.stats.walletType].iconStartTile;
            row1[1] = temp2 + 1;
            row2[0] = temp2 + 2;
            row2[1] = temp2 + 3;

            // palette change
            gPaletteBuffer[15 * 16 + 1] = walletColors[gSave.stats.walletType * 2];
            gPaletteBuffer[15 * 16 + 2] = walletColors[gSave.stats.walletType * 2 + 1];

            temp2 = 0xf070;
            row1[2] = temp2;
            row2[2] = temp2 + 1;
            row1[3] = temp2 + 2;
            row2[3] = temp2 + 3;
            row1[4] = temp2 + 4;
            row2[4] = temp2 + 5;
            if (digits > 3) {
                row1[5] = temp2 + 6;
                row2[5] = temp2 + 7;
            }
            if (digits > 4) {
                row1[6] = temp2 + 8;
                row2[6] = temp2 + 9;
            }
            gScreen.bg0.updated = 1;
            cVar1 = 1;
        } else {
            cVar1 = 0;
        }

        if (gUnk_0200AF00.rupees != gSave.stats.rupees) {
            if (gUnk_0200AF00.rupees < gSave.stats.rupees) {
                gUnk_0200AF00.rupees++;
            } else {
                gUnk_0200AF00.rupees--;
            }
            cVar1 = 2;
        }
        switch (cVar1) {
            case 2:
                temp = gUnk_0200AF00.unk_c;
                temp &= 3;
                if ((temp) == 0) {
                    SoundReq(SFX_RUPEE_GET);
                }
                // fallthrough
            case 1:
                RenderDigits(0x70, gUnk_0200AF00.rupees,
                             gWalletSizes[gSave.stats.walletType].size <= gUnk_0200AF00.rupees, digits);
                cVar1 = gUnk_0200AF00.unk_c + 1;
                // fallthrough
            default:
                gUnk_0200AF00.unk_c = cVar1;
                break;
        }
    }
}

extern const char* const historyNames[][40];

#define INLINE inline __attribute__((always_inline))

static INLINE char sanitize_char(char c) {
    if (c >= ' ' && c <= '_')
        return c;
    if (c >= 'a' && c <= 'z')
        return c - ('a' - 'A');
    return ' ';
}

static INLINE void DrawChar(char c, int x, int y) {
    gBG0Buffer[y * 32 + x] = c;
}

void DrawText(const char* text, int x, int y) {
    if (text) {
        for (int i = 0; text[i] != 0; i++) {
            DrawChar(sanitize_char(text[i]), x + i, y);
        }
    }
}

void DrawClear(int n, int x, int y) {
    for (int i = 0; i < n; i++) {
        DrawChar(0, x + i, y);
    }
}

void DrawNumber(u32 num, int digits, int x, int y) {
    int i = 0;
    while (digits > 0) {
        int p = pow(10, --digits);
        int digit = num / p;
        num = num % p;
        DrawChar('0' + digit, x + i, y);
        i++;
    }
}

typedef struct {
    u16 name;
    u16 displayTimer;
} HistoryTableEntry;

// is there a better way to introduce new ram symbols?
#define historyTable ((HistoryTableEntry*)0x0203F300)
#define historyTableEntries 12

extern u32 historyTimerDuration;

void UpdateHistoryTimer() {
    for (int i = 0; i < historyTableEntries; i++) {
        if (++historyTable[i].displayTimer >= historyTimerDuration) {
            historyTable[i].name = 0;
        }
    }
}

void DrawHistory(void) {
    if (!historyTimerDuration) {
        return;
    }

    int y = 18;
    int lines = historyTableEntries;
    if (TimerGetTime) {
        y--;
        lines--;
    }
    if (CounterGetValue) {
        y--;
        lines--;
    }

    if ((gUnk_0200AF00.unk_1 & 0x40) != 0) {
        for (int i = 0; i < lines; i++) {
            DrawClear(15, 1, y - i);
        }
    } else {
        UpdateHistoryTimer();
        for (int i = 0; i < lines; i++) {
            const char* name = historyNames[gSaveHeader->language][historyTable[i].name];
            if (name) {
                DrawClear(15, 1, y - i);
                DrawText(name, 1, y - i);
            } else {
                DrawClear(15, 1, y - i);
            }
        }
    }
    gScreen.bg0.updated = 1;
}

void DrawFigurineCounter(void) {
    if (!CounterGetValue) {
        return;
    }
    int y = 18;
    if (TimerGetTime) {
        y--;
    }
    if ((gUnk_0200AF00.unk_1 & 0x40) != 0) {
        DrawClear(7, 1, y);
        gScreen.bg0.updated = 1;
        return;
    }
    u32 value = CounterGetValue();
    u32 digits1 = log(value, 10) + 1;
    DrawNumber(value, digits1, 1, y);
    DrawChar('/', 1 + digits1, y);
    u32 digits2 = log(counterMaxValue, y) + 1;
    DrawNumber(counterMaxValue, digits2, 1 + digits1 + 1, y);
    gScreen.bg0.updated = 1;
}

// max display time is 99:59:59
// max timer value is 100:00:00 - 1 frame
// ((100*60*60*597275) / 10000 - 1)
#define TIMER_MAX 21501899

// draw timer
// uses custom charset
void DrawTimer(void) {
    if (!TimerGetTime) {
        return;
    }
    int x = 1;
    int y = 18;
    if ((gUnk_0200AF00.unk_1 & 0x40) != 0) {
        DrawClear(7, 1, y);
        gScreen.bg0.updated = 1;
        return;
    }
    u32 time = TimerGetTime();
    if (time > TIMER_MAX)
        time = TIMER_MAX;
    // 1 hour in frames at 0 decimals accuracy (perfect)
    // (60 * 60 * 597275)
    int hours = time / 215019;
    time = time % 215019;
    DrawChar(1 + hours / 10, x++, y);
    DrawChar(1 + hours % 10, x++, y);
    DrawChar(11, x++, y);
    time *= 100;
    // 1 minute in frames at 2 decimals accuracy (perfect)
    // (60 * 597275 / 100)
    int minutes = time / 358365;
    time = time % 358365;
    DrawChar(1 + minutes / 10, x++, y);
    DrawChar(1 + minutes % 10, x++, y);
    DrawChar(11, x++, y);
    time *= 100;
    // 1 seconds in frames at 4 decimals accuracy (perfect)
    int seconds = time / 597275;
    DrawChar(1 + seconds / 10, x++, y);
    DrawChar(1 + seconds % 10, x++, y);
    gScreen.bg0.updated = 1;
}

extern void UpdateUIElements(void);
void DrawHearts(void);
void DrawChargeBar(void);
void DrawKeys(void);

void DrawUI(void) {
    gUnk_0200AF00.unk_0 &= ~gUnk_0200AF00.unk_1;
    DrawHearts();
    DrawChargeBar();
    DrawRupees();
    DrawKeys();
    DrawHistory();
    DrawFigurineCounter();
    DrawTimer();
    gUnk_0200AF00.unk_0 = 0;
    UpdateUIElements();
}

extern void CreateUIElement(u32 type, u32 type2);

void InitUI(bool32 keepHealthAndRupees) {
    u32 health;
    u32 rupees;

    if (!keepHealthAndRupees) {
        health = gSave.stats.health >> 1;
        rupees = gSave.stats.rupees;
    } else {
        health = gUnk_0200AF00.health;
        rupees = gUnk_0200AF00.rupees;
    }
    MemClear(&gUnk_0200AF00, sizeof(struct_0200AF00));
    gUnk_0200AF00.health = health;
    gUnk_0200AF00.rupees = rupees;
    gUnk_0200AF00.maxHealth = gSave.stats.maxHealth >> 1;
    LoadPaletteGroup(0xc);
    LoadGfxGroup(0x10);
    MemClear(&gBG0Buffer, sizeof(gBG0Buffer));
    gScreen.bg0.tilemap = &gBG0Buffer;
    gScreen.bg0.control = 0x1f0c;
    gScreen.lcd.displayControl |= 0x100;
    gOAMControls.unk[0].unk7 = 1;
    gOAMControls.unk[0].unk6 = 1;
    gOAMControls.unk[1].unk6 = 1;
    gUnk_0200AF00.unk_13 = 0x7f;
    gUnk_0200AF00.unk_14 = 0x7f;
    gUnk_0200AF00.unk_8 = 0x7f;
    DrawHearts();
    DrawRupees();
    DrawChargeBar();
    DrawKeys();
    DrawHistory();
    DrawFigurineCounter();
    DrawTimer();
    gUnk_0200AF00.buttonX[0] = 0xd0;
    gUnk_0200AF00.buttonX[1] = 0xb8;
    gUnk_0200AF00.buttonX[2] = 0xd8;
    gUnk_0200AF00.buttonY[0] = 0x1c;
    gUnk_0200AF00.buttonY[1] = 0x1c;
    gUnk_0200AF00.buttonY[2] = 0xe;
    // TODO why is this array cleared again? Is it filled by the function calls in the mean time?
    MemClear(gUnk_0200AF00.elements, sizeof(gUnk_0200AF00.elements));
    CreateUIElement(UI_ELEMENT_TEXT_R, 9);
    CreateUIElement(UI_ELEMENT_ITEM_A, 0);
    CreateUIElement(UI_ELEMENT_ITEM_B, 0);
    CreateUIElement(UI_ELEMENT_BUTTON_R, 0);
    CreateUIElement(UI_ELEMENT_BUTTON_B, 0);
    CreateUIElement(UI_ELEMENT_BUTTON_A, 0);
    CreateUIElement(UI_ELEMENT_EZLONAGSTART, 0);
    CreateUIElement(UI_ELEMENT_HEART, 0);
}
