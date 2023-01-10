#include "player.h"
#include "save.h"

extern const s32 damageMultiplier;

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
    if (stats->maxHealth < newHealth) {
        newHealth = stats->maxHealth;
    }
    stats->health = newHealth;
    gPlayerEntity.health = newHealth;
    return newHealth;
}
