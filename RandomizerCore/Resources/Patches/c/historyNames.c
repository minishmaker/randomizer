#include "global.h"

typedef enum {
    LANGUAGE_JAPANESE,
    LANGUAGE_US,
    LANGUAGE_ENGLISH,
    LANGUAGE_FRENCH,
    LANGUAGE_GERMAN,
    LANGUAGE_SPANISH,
    LANGUAGE_ITALIAN,
} Language;

typedef enum {
    HISTORY_NONE,
    HISTORY_STONE_CLOUD,
    HISTORY_STONE_SWAMP,
    HISTORY_STONE_FALLS,
    HISTORY_DWS_BIG,
    HISTORY_DWS_SMALL,
    HISTORY_COF_BIG,
    HISTORY_COF_SMALL,
    HISTORY_FOW_BIG,
    HISTORY_FOW_SMALL,
    HISTORY_TOD_BIG,
    HISTORY_TOD_SMALL,
    HISTORY_POW_BIG,
    HISTORY_POW_SMALL,
    HISTORY_DHC_BIG,
    HISTORY_DHC_SMALL,
    HISTORY_RC_BIG,
    HISTORY_RC_SMALL,
    HISTORY_SCROLL_SPIN_ATTACK,
    HISTORY_SCROLL_ROLL_ATTACK,
    HISTORY_SCROLL_DASH_ATTACK,
    HISTORY_SCROLL_ROCK_BREAKER,
    HISTORY_SCROLL_SWORD_BEAM,
    HISTORY_SCROLL_GREAT_SPIN,
    HISTORY_SCROLL_DOWN_THRUST,
    HISTORY_SCROLL_PERIL_BEAM,
    HISTORY_BUTTERFLY_BOW,
    HISTORY_BUTTERFLY_DIG,
    HISTORY_BUTTERFLY_SWIM,
    HISTORY_SCROLL_FAST_SPIN,
    HISTORY_SCROLL_FAST_SPLIT,
    HISTORY_SCROLL_LONG_SPIN,
    HISTORY_STONE_RW,
    HISTORY_STONE_RV,
    HISTORY_STONE_RE,
    HISTORY_STONE_BL,
    HISTORY_STONE_BS,
    HISTORY_STONE_GC,
    HISTORY_STONE_GG,
    HISTORY_STONE_GP,
} HistoryName;

// each name may be 15 characters max
const char* const historyNames[][40] = {
    [LANGUAGE_JAPANESE]={
        [HISTORY_NONE] = NULL,
        [HISTORY_STONE_CLOUD] = "Cloud stone",
        [HISTORY_STONE_SWAMP] = "Swamp stone",
        [HISTORY_STONE_FALLS] = "Falls stone",
        [HISTORY_DWS_BIG] = "DWS Big",
        [HISTORY_DWS_SMALL] = "DWS Small",
        [HISTORY_COF_BIG] = "COF Big",
        [HISTORY_COF_SMALL] = "COF Small",
        [HISTORY_FOW_BIG] = "FOW Big",
        [HISTORY_FOW_SMALL] = "FOW Small",
        [HISTORY_TOD_BIG] = "TOD Big",
        [HISTORY_TOD_SMALL] = "TOD Small",
        [HISTORY_POW_BIG] = "POW Big",
        [HISTORY_POW_SMALL] = "POW Small",
        [HISTORY_DHC_BIG] = "DHC Big",
        [HISTORY_DHC_SMALL] = "DHC Small",
        [HISTORY_RC_BIG] = "RC  Big",
        [HISTORY_RC_SMALL] = "RC  Small",
        [HISTORY_SCROLL_SPIN_ATTACK] = "Spin attack",
        [HISTORY_SCROLL_ROLL_ATTACK] = "Roll attack",
        [HISTORY_SCROLL_DASH_ATTACK] = "Dash attack",
        [HISTORY_SCROLL_ROCK_BREAKER] = "Rock breaker",
        [HISTORY_SCROLL_SWORD_BEAM] = "Sword beam",
        [HISTORY_SCROLL_GREAT_SPIN] = "Great spin",
        [HISTORY_SCROLL_DOWN_THRUST] = "Down thrust",
        [HISTORY_SCROLL_PERIL_BEAM] = "Peril beam",
        [HISTORY_BUTTERFLY_BOW] = "Bow butterfly",
        [HISTORY_BUTTERFLY_DIG] = "Dig butterfly",
        [HISTORY_BUTTERFLY_SWIM] = "Swim butterfly",
        [HISTORY_SCROLL_FAST_SPIN] = "Fast spin",
        [HISTORY_SCROLL_FAST_SPLIT] = "Fast split",
        [HISTORY_SCROLL_LONG_SPIN] = "Long spin",
        [HISTORY_STONE_RW] = "Red W stone",
        [HISTORY_STONE_RV] = "Red V stone",
        [HISTORY_STONE_RE] = "Red E stone",
        [HISTORY_STONE_BL] = "Blue L stone",
        [HISTORY_STONE_BS] = "Blue S stone",
        [HISTORY_STONE_GC] = "Green C stone",
        [HISTORY_STONE_GG] = "Green G stone",
        [HISTORY_STONE_GP] = "Green P stone",
    },
    [LANGUAGE_US]={
        [HISTORY_NONE] = NULL,
        [HISTORY_STONE_CLOUD] = "Cloud stone",
        [HISTORY_STONE_SWAMP] = "Swamp stone",
        [HISTORY_STONE_FALLS] = "Falls stone",
        [HISTORY_DWS_BIG] = "DWS Big",
        [HISTORY_DWS_SMALL] = "DWS Small",
        [HISTORY_COF_BIG] = "COF Big",
        [HISTORY_COF_SMALL] = "COF Small",
        [HISTORY_FOW_BIG] = "FOW Big",
        [HISTORY_FOW_SMALL] = "FOW Small",
        [HISTORY_TOD_BIG] = "TOD Big",
        [HISTORY_TOD_SMALL] = "TOD Small",
        [HISTORY_POW_BIG] = "POW Big",
        [HISTORY_POW_SMALL] = "POW Small",
        [HISTORY_DHC_BIG] = "DHC Big",
        [HISTORY_DHC_SMALL] = "DHC Small",
        [HISTORY_RC_BIG] = "RC  Big",
        [HISTORY_RC_SMALL] = "RC  Small",
        [HISTORY_SCROLL_SPIN_ATTACK] = "Spin attack",
        [HISTORY_SCROLL_ROLL_ATTACK] = "Roll attack",
        [HISTORY_SCROLL_DASH_ATTACK] = "Dash attack",
        [HISTORY_SCROLL_ROCK_BREAKER] = "Rock breaker",
        [HISTORY_SCROLL_SWORD_BEAM] = "Sword beam",
        [HISTORY_SCROLL_GREAT_SPIN] = "Great spin",
        [HISTORY_SCROLL_DOWN_THRUST] = "Down thrust",
        [HISTORY_SCROLL_PERIL_BEAM] = "Peril beam",
        [HISTORY_BUTTERFLY_BOW] = "Bow butterfly",
        [HISTORY_BUTTERFLY_DIG] = "Dig butterfly",
        [HISTORY_BUTTERFLY_SWIM] = "Swim butterfly",
        [HISTORY_SCROLL_FAST_SPIN] = "Fast spin",
        [HISTORY_SCROLL_FAST_SPLIT] = "Fast split",
        [HISTORY_SCROLL_LONG_SPIN] = "Long spin",
        [HISTORY_STONE_RW] = "Red W stone",
        [HISTORY_STONE_RV] = "Red V stone",
        [HISTORY_STONE_RE] = "Red E stone",
        [HISTORY_STONE_BL] = "Blue L stone",
        [HISTORY_STONE_BS] = "Blue S stone",
        [HISTORY_STONE_GC] = "Green C stone",
        [HISTORY_STONE_GG] = "Green G stone",
        [HISTORY_STONE_GP] = "Green P stone",
    },
    [LANGUAGE_ENGLISH]={
        [HISTORY_NONE] = NULL,
        [HISTORY_STONE_CLOUD] = "Cloud stone",
        [HISTORY_STONE_SWAMP] = "Swamp stone",
        [HISTORY_STONE_FALLS] = "Falls stone",
        [HISTORY_DWS_BIG] = "DWS Big",
        [HISTORY_DWS_SMALL] = "DWS Small",
        [HISTORY_COF_BIG] = "COF Big",
        [HISTORY_COF_SMALL] = "COF Small",
        [HISTORY_FOW_BIG] = "FOW Big",
        [HISTORY_FOW_SMALL] = "FOW Small",
        [HISTORY_TOD_BIG] = "TOD Big",
        [HISTORY_TOD_SMALL] = "TOD Small",
        [HISTORY_POW_BIG] = "POW Big",
        [HISTORY_POW_SMALL] = "POW Small",
        [HISTORY_DHC_BIG] = "DHC Big",
        [HISTORY_DHC_SMALL] = "DHC Small",
        [HISTORY_RC_BIG] = "RC  Big",
        [HISTORY_RC_SMALL] = "RC  Small",
        [HISTORY_SCROLL_SPIN_ATTACK] = "Spin attack",
        [HISTORY_SCROLL_ROLL_ATTACK] = "Roll attack",
        [HISTORY_SCROLL_DASH_ATTACK] = "Dash attack",
        [HISTORY_SCROLL_ROCK_BREAKER] = "Rock breaker",
        [HISTORY_SCROLL_SWORD_BEAM] = "Sword beam",
        [HISTORY_SCROLL_GREAT_SPIN] = "Great spin",
        [HISTORY_SCROLL_DOWN_THRUST] = "Down thrust",
        [HISTORY_SCROLL_PERIL_BEAM] = "Peril beam",
        [HISTORY_BUTTERFLY_BOW] = "Bow butterfly",
        [HISTORY_BUTTERFLY_DIG] = "Dig butterfly",
        [HISTORY_BUTTERFLY_SWIM] = "Swim butterfly",
        [HISTORY_SCROLL_FAST_SPIN] = "Fast spin",
        [HISTORY_SCROLL_FAST_SPLIT] = "Fast split",
        [HISTORY_SCROLL_LONG_SPIN] = "Long spin",
        [HISTORY_STONE_RW] = "Red W stone",
        [HISTORY_STONE_RV] = "Red V stone",
        [HISTORY_STONE_RE] = "Red E stone",
        [HISTORY_STONE_BL] = "Blue L stone",
        [HISTORY_STONE_BS] = "Blue S stone",
        [HISTORY_STONE_GC] = "Green C stone",
        [HISTORY_STONE_GG] = "Green G stone",
        [HISTORY_STONE_GP] = "Green P stone",
    },
    [LANGUAGE_FRENCH]={
        [HISTORY_NONE] = NULL,
        [HISTORY_STONE_CLOUD] = "Cloud stone",
        [HISTORY_STONE_SWAMP] = "Swamp stone",
        [HISTORY_STONE_FALLS] = "Falls stone",
        [HISTORY_DWS_BIG] = "DWS Big",
        [HISTORY_DWS_SMALL] = "DWS Small",
        [HISTORY_COF_BIG] = "COF Big",
        [HISTORY_COF_SMALL] = "COF Small",
        [HISTORY_FOW_BIG] = "FOW Big",
        [HISTORY_FOW_SMALL] = "FOW Small",
        [HISTORY_TOD_BIG] = "TOD Big",
        [HISTORY_TOD_SMALL] = "TOD Small",
        [HISTORY_POW_BIG] = "POW Big",
        [HISTORY_POW_SMALL] = "POW Small",
        [HISTORY_DHC_BIG] = "DHC Big",
        [HISTORY_DHC_SMALL] = "DHC Small",
        [HISTORY_RC_BIG] = "RC  Big",
        [HISTORY_RC_SMALL] = "RC  Small",
        [HISTORY_SCROLL_SPIN_ATTACK] = "Spin attack",
        [HISTORY_SCROLL_ROLL_ATTACK] = "Roll attack",
        [HISTORY_SCROLL_DASH_ATTACK] = "Dash attack",
        [HISTORY_SCROLL_ROCK_BREAKER] = "Rock breaker",
        [HISTORY_SCROLL_SWORD_BEAM] = "Sword beam",
        [HISTORY_SCROLL_GREAT_SPIN] = "Great spin",
        [HISTORY_SCROLL_DOWN_THRUST] = "Down thrust",
        [HISTORY_SCROLL_PERIL_BEAM] = "Peril beam",
        [HISTORY_BUTTERFLY_BOW] = "Bow butterfly",
        [HISTORY_BUTTERFLY_DIG] = "Dig butterfly",
        [HISTORY_BUTTERFLY_SWIM] = "Swim butterfly",
        [HISTORY_SCROLL_FAST_SPIN] = "Fast spin",
        [HISTORY_SCROLL_FAST_SPLIT] = "Fast split",
        [HISTORY_SCROLL_LONG_SPIN] = "Long spin",
        [HISTORY_STONE_RW] = "Red W stone",
        [HISTORY_STONE_RV] = "Red V stone",
        [HISTORY_STONE_RE] = "Red E stone",
        [HISTORY_STONE_BL] = "Blue L stone",
        [HISTORY_STONE_BS] = "Blue S stone",
        [HISTORY_STONE_GC] = "Green C stone",
        [HISTORY_STONE_GG] = "Green G stone",
        [HISTORY_STONE_GP] = "Green P stone",
    },
    [LANGUAGE_GERMAN]={
        [HISTORY_NONE] = NULL,
        [HISTORY_STONE_CLOUD] = "Cloud stone",
        [HISTORY_STONE_SWAMP] = "Swamp stone",
        [HISTORY_STONE_FALLS] = "Falls stone",
        [HISTORY_DWS_BIG] = "DWS Big",
        [HISTORY_DWS_SMALL] = "DWS Small",
        [HISTORY_COF_BIG] = "COF Big",
        [HISTORY_COF_SMALL] = "COF Small",
        [HISTORY_FOW_BIG] = "FOW Big",
        [HISTORY_FOW_SMALL] = "FOW Small",
        [HISTORY_TOD_BIG] = "TOD Big",
        [HISTORY_TOD_SMALL] = "TOD Small",
        [HISTORY_POW_BIG] = "POW Big",
        [HISTORY_POW_SMALL] = "POW Small",
        [HISTORY_DHC_BIG] = "DHC Big",
        [HISTORY_DHC_SMALL] = "DHC Small",
        [HISTORY_RC_BIG] = "RC  Big",
        [HISTORY_RC_SMALL] = "RC  Small",
        [HISTORY_SCROLL_SPIN_ATTACK] = "Spin attack",
        [HISTORY_SCROLL_ROLL_ATTACK] = "Roll attack",
        [HISTORY_SCROLL_DASH_ATTACK] = "Dash attack",
        [HISTORY_SCROLL_ROCK_BREAKER] = "Rock breaker",
        [HISTORY_SCROLL_SWORD_BEAM] = "Sword beam",
        [HISTORY_SCROLL_GREAT_SPIN] = "Great spin",
        [HISTORY_SCROLL_DOWN_THRUST] = "Down thrust",
        [HISTORY_SCROLL_PERIL_BEAM] = "Peril beam",
        [HISTORY_BUTTERFLY_BOW] = "Bow butterfly",
        [HISTORY_BUTTERFLY_DIG] = "Dig butterfly",
        [HISTORY_BUTTERFLY_SWIM] = "Swim butterfly",
        [HISTORY_SCROLL_FAST_SPIN] = "Fast spin",
        [HISTORY_SCROLL_FAST_SPLIT] = "Fast split",
        [HISTORY_SCROLL_LONG_SPIN] = "Long spin",
        [HISTORY_STONE_RW] = "Red W stone",
        [HISTORY_STONE_RV] = "Red V stone",
        [HISTORY_STONE_RE] = "Red E stone",
        [HISTORY_STONE_BL] = "Blue L stone",
        [HISTORY_STONE_BS] = "Blue S stone",
        [HISTORY_STONE_GC] = "Green C stone",
        [HISTORY_STONE_GG] = "Green G stone",
        [HISTORY_STONE_GP] = "Green P stone",
    },
    [LANGUAGE_SPANISH]={
        [HISTORY_NONE] = NULL,
        [HISTORY_STONE_CLOUD] = "Cloud stone",
        [HISTORY_STONE_SWAMP] = "Swamp stone",
        [HISTORY_STONE_FALLS] = "Falls stone",
        [HISTORY_DWS_BIG] = "DWS Big",
        [HISTORY_DWS_SMALL] = "DWS Small",
        [HISTORY_COF_BIG] = "COF Big",
        [HISTORY_COF_SMALL] = "COF Small",
        [HISTORY_FOW_BIG] = "FOW Big",
        [HISTORY_FOW_SMALL] = "FOW Small",
        [HISTORY_TOD_BIG] = "TOD Big",
        [HISTORY_TOD_SMALL] = "TOD Small",
        [HISTORY_POW_BIG] = "POW Big",
        [HISTORY_POW_SMALL] = "POW Small",
        [HISTORY_DHC_BIG] = "DHC Big",
        [HISTORY_DHC_SMALL] = "DHC Small",
        [HISTORY_RC_BIG] = "RC  Big",
        [HISTORY_RC_SMALL] = "RC  Small",
        [HISTORY_SCROLL_SPIN_ATTACK] = "Spin attack",
        [HISTORY_SCROLL_ROLL_ATTACK] = "Roll attack",
        [HISTORY_SCROLL_DASH_ATTACK] = "Dash attack",
        [HISTORY_SCROLL_ROCK_BREAKER] = "Rock breaker",
        [HISTORY_SCROLL_SWORD_BEAM] = "Sword beam",
        [HISTORY_SCROLL_GREAT_SPIN] = "Great spin",
        [HISTORY_SCROLL_DOWN_THRUST] = "Down thrust",
        [HISTORY_SCROLL_PERIL_BEAM] = "Peril beam",
        [HISTORY_BUTTERFLY_BOW] = "Bow butterfly",
        [HISTORY_BUTTERFLY_DIG] = "Dig butterfly",
        [HISTORY_BUTTERFLY_SWIM] = "Swim butterfly",
        [HISTORY_SCROLL_FAST_SPIN] = "Fast spin",
        [HISTORY_SCROLL_FAST_SPLIT] = "Fast split",
        [HISTORY_SCROLL_LONG_SPIN] = "Long spin",
        [HISTORY_STONE_RW] = "Red W stone",
        [HISTORY_STONE_RV] = "Red V stone",
        [HISTORY_STONE_RE] = "Red E stone",
        [HISTORY_STONE_BL] = "Blue L stone",
        [HISTORY_STONE_BS] = "Blue S stone",
        [HISTORY_STONE_GC] = "Green C stone",
        [HISTORY_STONE_GG] = "Green G stone",
        [HISTORY_STONE_GP] = "Green P stone",
    },
    [LANGUAGE_ITALIAN]={
        [HISTORY_NONE] = NULL,
        [HISTORY_STONE_CLOUD] = "Cloud stone",
        [HISTORY_STONE_SWAMP] = "Swamp stone",
        [HISTORY_STONE_FALLS] = "Falls stone",
        [HISTORY_DWS_BIG] = "DWS Big",
        [HISTORY_DWS_SMALL] = "DWS Small",
        [HISTORY_COF_BIG] = "COF Big",
        [HISTORY_COF_SMALL] = "COF Small",
        [HISTORY_FOW_BIG] = "FOW Big",
        [HISTORY_FOW_SMALL] = "FOW Small",
        [HISTORY_TOD_BIG] = "TOD Big",
        [HISTORY_TOD_SMALL] = "TOD Small",
        [HISTORY_POW_BIG] = "POW Big",
        [HISTORY_POW_SMALL] = "POW Small",
        [HISTORY_DHC_BIG] = "DHC Big",
        [HISTORY_DHC_SMALL] = "DHC Small",
        [HISTORY_RC_BIG] = "RC  Big",
        [HISTORY_RC_SMALL] = "RC  Small",
        [HISTORY_SCROLL_SPIN_ATTACK] = "Spin attack",
        [HISTORY_SCROLL_ROLL_ATTACK] = "Roll attack",
        [HISTORY_SCROLL_DASH_ATTACK] = "Dash attack",
        [HISTORY_SCROLL_ROCK_BREAKER] = "Rock breaker",
        [HISTORY_SCROLL_SWORD_BEAM] = "Sword beam",
        [HISTORY_SCROLL_GREAT_SPIN] = "Great spin",
        [HISTORY_SCROLL_DOWN_THRUST] = "Down thrust",
        [HISTORY_SCROLL_PERIL_BEAM] = "Peril beam",
        [HISTORY_BUTTERFLY_BOW] = "Bow butterfly",
        [HISTORY_BUTTERFLY_DIG] = "Dig butterfly",
        [HISTORY_BUTTERFLY_SWIM] = "Swim butterfly",
        [HISTORY_SCROLL_FAST_SPIN] = "Fast spin",
        [HISTORY_SCROLL_FAST_SPLIT] = "Fast split",
        [HISTORY_SCROLL_LONG_SPIN] = "Long spin",
        [HISTORY_STONE_RW] = "Red W stone",
        [HISTORY_STONE_RV] = "Red V stone",
        [HISTORY_STONE_RE] = "Red E stone",
        [HISTORY_STONE_BL] = "Blue L stone",
        [HISTORY_STONE_BS] = "Blue S stone",
        [HISTORY_STONE_GC] = "Green C stone",
        [HISTORY_STONE_GG] = "Green G stone",
        [HISTORY_STONE_GP] = "Green P stone",
    },
};