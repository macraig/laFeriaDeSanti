using Assets.Scripts.Games;
using Assets.Scripts.Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaulSoundStage : RaulStage
{

    private AudioClip[][] soundsToPlay;
    private AudioClip[] sounds;

    private int lastValueSelected;
    public RaulSoundStage(AudioClip[] sounds)
    {

        this.sounds = sounds;

        soundsToPlay = new AudioClip[4][];


       
    }


    public void PlaySound(int random)
    {
        lastValueSelected = random;
        if(soundsToPlay[random].Length == 1)
        {
            SoundController.GetController().PlayClip(soundsToPlay[random][0]);
        }else
        {
            List<AudioClip> audiosToPlay = new List<AudioClip>();

            audiosToPlay.Add(soundsToPlay[random][0]);
            audiosToPlay.Add(soundsToPlay[random][1]);

            SoundController.GetController().ConcatenateAudios(audiosToPlay,null);
        }
    }

    public override void ShowNextEnunciado(int randomResult,  Sprite[] restAnimalEnunciado, Sprite[] restAnimalResultado)
    {
        PlaySound(randomResult);
        RaulSaysController.instance.view.ShowSoundOption(randomResult, restAnimalEnunciado, restAnimalResultado);
    }

    public override void SetView()
    {
        RaulSaysController.instance.view.SetAudioStage();
    }

    public void RepeatLastSound()
    {
        PlaySound(lastValueSelected);
    }

    public override void UpdateLevelValues(int currentLevel)
    {
        if (currentLevel == 0)
        {

            for (int i = 0; i < sounds.Length; i++)
            {
                soundsToPlay[i] = new AudioClip[1];
                soundsToPlay[i][0] = sounds[i];
            }
        }

        else if(currentLevel == 1)
        {

            for (int i = 0; i < sounds.Length; i++)
            {
                soundsToPlay[i] = new AudioClip[2];
            }

            soundsToPlay[0][0] = sounds[0];
            soundsToPlay[0][1] = sounds[2];

            soundsToPlay[1][0] = sounds[0];
            soundsToPlay[1][1] = sounds[3];

            soundsToPlay[2][0] = sounds[1];
            soundsToPlay[2][1] = sounds[2];

            soundsToPlay[3][0] = sounds[1];
            soundsToPlay[3][1] = sounds[3];
        }else
        {
            soundsToPlay = new AudioClip[8][];


            for (int i = 0; i < 8; i++)
            {

                if (i < 4)
                {
                    soundsToPlay[i] = new AudioClip[1];
                    soundsToPlay[i][0] = sounds[i];
                }
                else
                {
                    soundsToPlay[i] = new AudioClip[2];
                }

            }

            soundsToPlay[4][0] = sounds[0];
            soundsToPlay[4][1] = sounds[2];

            soundsToPlay[5][0] = sounds[0];
            soundsToPlay[5][1] = sounds[3];

            soundsToPlay[6][0] = sounds[1];
            soundsToPlay[6][1] = sounds[2];

            soundsToPlay[7][0] = sounds[1];
            soundsToPlay[7][1] = sounds[3];
        }
    }
}
