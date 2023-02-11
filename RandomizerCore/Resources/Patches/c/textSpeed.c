#include "save.h"
#include "common.h"
#include "fileselect.h"

void ResetSaveFile(u32 save_idx) {
    SaveFile* save;

    gMapDataBottomSpecial.saveStatus[save_idx] = 0;
    save = &gMapDataBottomSpecial.saves[save_idx];
    MemClear(save, sizeof(SaveFile));
    save->msg_speed = 2;
    save->brightness = 1;
    save->stats.health = 24;
    save->stats.maxHealth = 24;
}
