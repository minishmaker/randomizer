#include "player.h"
#include "save.h"

extern const s32 damageMultiplier;
extern int (*const CheckOHKO)(void);

int OHKOTrue(void) {
    return 1;
}

s32 ModHealth(s32 delta) {
    s32 newHealth;
    Stats* stats = &gSave.stats;

    if (delta < 0) {
        delta *= damageMultiplier;
    }

    newHealth = stats->health + delta;
    if (newHealth < 0) {
        newHealth = 0;
    }
    if (newHealth > stats->maxHealth) {
        newHealth = stats->maxHealth;
    }

    if (CheckOHKO && (newHealth <= stats->health) && CheckOHKO()) {
        newHealth = 0;
    }

    stats->health = newHealth;
    gPlayerEntity.health = newHealth;
    return newHealth;
}
