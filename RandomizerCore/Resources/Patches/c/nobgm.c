#include "sound.h"
#include "gba/m4a.h"
#include "main.h"

#define IS_BGM(song) ((song)-1 <= NUM_BGM - 1)
#define IS_SFX(song) ((song)-1 > NUM_BGM - 1)

extern s32 fade(s32 target, s32 current);
extern void doPlaySound(u32 sound);
extern void PlayFadeIn(u32 sound);
extern void PlayFadeOut(u32 sound);
extern void InitVolume(void);
extern void SetMasterVolume(u32 volume);

void SoundReq(u32 sound) {
    u32 song;
    SoundPlayingInfo* ptr;
    if (gMain.field_0x7)
        return;
    ptr = &gSoundPlayingInfo;
    song = sound & 0xffff;
    switch (sound & 0xffff0000) {
        case SONG_STOP_ALL:
            ptr->currentBgm = 0;
            m4aMPlayAllStop();
            return;
        case SONG_MUTE:
            SetMasterVolume(0);
            return;
        case SONG_PLAY_VOL_RESET_ALL:
            InitVolume();
            ptr->volumeSfx = 0x100;
            doPlaySound(ptr->currentBgm);
            return;
        case SONG_VOL_FADE_OUT:
            PlayFadeOut(ptr->currentBgm);
            return;
        case SONG_FADE_IN:
            return;
            if (song == 0)
                song = ptr->currentBgm;
            if (IS_SFX(song))
                return;
            ptr->currentBgm = song;
            m4aSongNumStart(song);
            PlayFadeIn(song);
            return;
        case SONG_FADE_IN_CONTINUE:
            return;
            if (IS_SFX(song))
                return;
            ptr->currentBgm = song;
            m4aSongNumStartOrContinue(song);
            PlayFadeIn(song);
            return;
        case SONG_PLAY_TEMPO_CONTROL:
            m4aMPlayTempoControl(gMusicPlayers[gSongTable[ptr->currentBgm].musicPlayerIndex].info, song);
            return;
        case SONG_VSYNC_OFF:
            m4aMPlayAllStop();
            m4aSoundVSyncOff();
            return;
        case SONG_STOP:
            if (ptr->currentBgm == 0)
                return;
            m4aSongNumStop(ptr->currentBgm);
            return;
        case SONG_VSYNC_ON:
            m4aSoundVSyncOn();
            // fallthrough
        case SONG_CONTINUE:
            if (ptr->currentBgm == 0)
                return;
            m4aSongNumStartOrContinue(ptr->currentBgm);
            doPlaySound(ptr->currentBgm);
            return;
        case SONG_PLAY_VOL_RESET:
            return;
            if (IS_SFX(song))
                return;
            ptr->currentBgm = song;
            m4aSongNumStartOrContinue(song);
            InitVolume();
            doPlaySound(song);
            return;
        case SONG_FADE_OUT_BGM:
            ptr->volumeBgmTarget = 0;
            return;
        case SONG_STOP_BGM:
            ptr->volumeBgmTarget = 0;
            ptr->stopBgm = TRUE;
            return;
        case SONG_FADE_IN_BGM:
            ptr->volumeBgmTarget = 0x100;
            return;
        case SONG_INIT:
            InitVolume();
            return;
        case SONG_BGM_0:
            ptr->currentBgm = 0;
            return;
        default:
            if (song != 0) {
                if (IS_BGM(song)) {
                    return;
                    ptr->currentBgm = song;
                    m4aSongNumStart(song);
                    InitVolume();
                } else {
                    m4aSongNumStart(song);
                }
                doPlaySound(song);
            }
            return;
    }
}
