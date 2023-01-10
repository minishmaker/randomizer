#include "global.h"
#include "common.h"
#include "sound.h"
#include "save.h"
#include "screen.h"

extern u8 RupeeKeyDigits[];

void RenderDigits(u32 iconVramIndex, u32 count, u32 isTextYellow, u32 digits) {
    int iVar2;
    int iVar3;
    u8* puVar4;
    u32 digit;
    register u32 r1 asm("r1");

    puVar4 = RupeeKeyDigits;
    if (isTextYellow == 0) {
        puVar4 -= 0x280;
    }
    iVar3 = (iconVramIndex & 0x3ff) * 0x20;
    iVar2 = iVar3 + 0x600c000;
    switch (digits) {
        case 5:
            digit = Div(count, 10000);
            count = r1;
            DmaCopy32(3, puVar4 + digit * 0x40, iVar2, 0x10 * 4);
            iVar2 += 0x40;
            // fallthrough
        case 4:
            digit = Div(count, 1000);
            count = r1;
            DmaCopy32(3, puVar4 + digit * 0x40, iVar2, 0x10 * 4);
            iVar2 += 0x40;
            // fallthrough
        case 3:
            digit = Div(count, 100);
            count = r1;
            DmaCopy32(3, puVar4 + digit * 0x40, iVar2, 0x10 * 4);
            iVar2 += 0x40;
            // fallthrough
        case 2:
            digit = Div(count, 10);
            count = r1;
            DmaCopy32(3, puVar4 + digit * 0x40, iVar2, 0x10 * 4);
            iVar2 += 0x40;
    }

    DmaCopy32(3, puVar4 + count * 0x40, iVar2, 0x10 * 4);
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

    int digits = gSave.stats.walletType > 1 ? 4 : 3;

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

typedef enum {
    HISTORY_NONE,
    cloudHistory,
    swampHistory,
    fallsHistory,
    bossHistoryDWS,
    smolHistoryDWS,
    bossHistoryCOF,
    smolHistoryCOF,
    bossHistoryFOW,
    smolHistoryFOW,
    bossHistoryTOD,
    smolHistoryTOD,
    bossHistoryPOW,
    smolHistoryPOW,
    bossHistoryDHC,
    smolHistoryDHC,
    bossHistoryRC,
    smolHistoryRC,
    scrollHistorySpinAttack,
    scrollHistoryRollAttack,
    scrollHistoryDashAttack,
    scrollHistoryRockBreaker,
    scrollHistorySwordBeam,
    scrollHistoryGreatSpin,
    scrollHistoryDownThrust,
    scrollHistoryPerilBeam,
    butterflyHistoryBow,
    butterflyHistoryDig,
    butterflyHistorySwim,
    scrollHistoryFastSpin,
    scrollHistoryFastSplit,
    scrollHistoryLongSpin,
    kinstoneHistoryRedW,
    kinstoneHistoryRedV,
    kinstoneHistoryRedE,
    kinstoneHistoryBlueL,
    kinstoneHistoryBlueS,
    kinstoneHistoryGreenC,
    kinstoneHistoryGreenG,
    kinstoneHistoryGreenP,
} HistoryName;

const char* const historyNames[] = {
    [HISTORY_NONE] = NULL,
    [cloudHistory] = "Cloud stone",
    [swampHistory] = "Swamp stone",
    [fallsHistory] = "Falls stone",
    [bossHistoryDWS] = "DWS Big",
    [smolHistoryDWS] = "DWS Small",
    [bossHistoryCOF] = "COF Big",
    [smolHistoryCOF] = "COF Small",
    [bossHistoryFOW] = "FOW Big",
    [smolHistoryFOW] = "FOW Small",
    [bossHistoryTOD] = "TOD Big",
    [smolHistoryTOD] = "TOD Small",
    [bossHistoryPOW] = "POW Big",
    [smolHistoryPOW] = "POW Small",
    [bossHistoryDHC] = "DHC Big",
    [smolHistoryDHC] = "DHC Small",
    [bossHistoryRC] = "RC  Big",
    [smolHistoryRC] = "RC  Small",
    [scrollHistorySpinAttack] = "Spin attack",
    [scrollHistoryRollAttack] = "Roll attack",
    [scrollHistoryDashAttack] = "Dash attack",
    [scrollHistoryRockBreaker] = "Rock breaker",
    [scrollHistorySwordBeam] = "Sword beam",
    [scrollHistoryGreatSpin] = "Great spin",
    [scrollHistoryDownThrust] = "Down thrust",
    [scrollHistoryPerilBeam] = "Peril beam",
    [butterflyHistoryBow] = "Bow butterfly",
    [butterflyHistoryDig] = "Dig butterfly",
    [butterflyHistorySwim] = "Swim butterfly",
    [scrollHistoryFastSpin] = "Fast spin",
    [scrollHistoryFastSplit] = "Fast split",
    [scrollHistoryLongSpin] = "Long spin",
    [kinstoneHistoryRedW] = "Red W stone",
    [kinstoneHistoryRedV] = "Red V stone",
    [kinstoneHistoryRedE] = "Red E stone",
    [kinstoneHistoryBlueL] = "Blue L stone",
    [kinstoneHistoryBlueS] = "Blue S stone",
    [kinstoneHistoryGreenC] = "Green C stone",
    [kinstoneHistoryGreenG] = "Green G stone",
    [kinstoneHistoryGreenP] = "Green P stone",
};

#define INLINE inline __attribute__((always_inline))

static INLINE char sanitize_char(char c) {
    if (c >= ' ' && c <= '_')
        return c;
    if (c >= 'a' && c <= 'z')
        return c - ('a' - 'A');
    return ' ';
}

void DrawText(const char* text, int x, int y) {
    if (text) {
        for (int i = 0; text[i] != 0; i++) {
            gBG0Buffer[y * 32 + x + i] = sanitize_char(text[i]);
        }
    } else {
        for (int i = 0; i < 15; i++) {
            gBG0Buffer[y * 32 + x + i] = 0;
        }
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
    if ((gUnk_0200AF00.unk_1 & 0x40) != 0) {
        for (int i = 0; i < historyTableEntries; i++) {
            DrawText(NULL, 0, 19 - i);
        }
    } else {
        UpdateHistoryTimer();
        for (int i = 0; i < historyTableEntries; i++) {
            DrawText(historyNames[historyTable[i].name], 0, 19 - i);
        }
    }
    gScreen.bg0.updated = 1;
}

void DrawFigurineCounter(void) {
}

void DrawTimer(void) {
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
