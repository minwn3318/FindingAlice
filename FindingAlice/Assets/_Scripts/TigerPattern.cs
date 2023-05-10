﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerPattern : MonoBehaviour
{
    public GameObject pattern;
    public GameObject rock;
    public GameObject claw;

    GameObject player;
    GameObject tiger;
    GameObject tigerPlatform;
    GameObject tigerGround;

    Renderer patternColor;
    Animator anim;

    //1번 패턴인지 2번 패턴인지 랜덤값을 저장
    int patternValue;
    //패턴 재발동 쿨타임
    float patternCooldown = 5f;
    //현재 패턴이 재생 중인지 확인
    bool isPatternPlay = false;

    [HideInInspector] public bool checking = false;

    float startDelay = 2.0f;

    //패턴 1
    //돌 떨어지는 5개 자리 지정
    int[] pattern1_order = new int[5];
    //몇번째 돌이 떨어지는지 저장
    int pattern1_count;
    float pattern1_time = 0;
    float pattern1_Delay = 1.0f;

    //패턴 2
    //패턴 예고가 그려지는 시간
    float pattern2_launchTime = 3f;
    //시간 비율을 저장하는 변수
    float pattern2_duration;
    float pattern2_time;

    EffectSound SFXSound;

    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        patternColor = pattern.GetComponent<Renderer>();
        //pattern.transform.localScale = new Vector3(3, 100, 0);
        tiger = transform.Find("Tiger").gameObject;
        anim = tiger.GetComponent<Animator>();
        tigerPlatform = transform.Find("TigerPlatform").gameObject;
        tigerGround = this.gameObject;
        SFXSound = GameObject.Find("EffectSound").gameObject.GetComponent<EffectSound>();
    }

    private void Update()
    {
        transform.position = new Vector3(player.transform.position.x,
                                         this.transform.position.y,
                                         this.transform.position.z);

        //테스팅 용
        //if (Input.GetKeyDown(KeyCode.P))
        //    PatternPlay();
        //if (Input.GetKeyDown(KeyCode.E))
        //    PatternExit();

        if (tigerGround)
        {
            if (!checking)
            {
                PatternPlay();
                checking = true;
            }
        }    
        else if (!tigerGround)
        {
            if (checking)
            {
                PatternExit();
                checking = false;
            }
        }
            

    }

    public void PatternPlay()
    {
        if (!isPatternPlay)
        {
            tiger.SetActive(true);
            tigerPlatform.SetActive(true);
            StartCoroutine(Pattern());
        }
    }

    public void PatternExit()
    {
        //if (isPatternPlay)
        //{
        tiger.SetActive(false);
        tigerPlatform.SetActive(false);
        StopCoroutine(Pattern());
        //}
    }

    IEnumerator Pattern()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            pattern.transform.parent = claw.transform.parent = null;
            pattern.SetActive(true);
            patternValue = Random.Range(0, 2);
            pattern2_time = 0;
            pattern2_duration = 0;
            SFXSound.PlaySFX(17000);

            //패턴 1
            if (patternValue == 0)
            {
                for (int i = 0; i < 5; i++)
                    pattern1_order[i] = Random.Range(0, 5);

                pattern1_count = 0;
                pattern.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                patternColor.material.color = new Color(1, 0, 0, 0.5f);

				while (pattern1_count < 5)
                {
                    anim.SetTrigger("doThrow");
                    pattern.transform.position = new Vector3(player.transform.position.x + ((pattern1_order[pattern1_count] - 2) * 3),
                                                                player.transform.position.y,
                                                                pattern.transform.position.z);
                    yield return new WaitForSeconds(pattern1_Delay);
                    //돌 떨구는 사운드 1회 - 4초 가량으로 5번 째 돌 떨어지기 전에 끝남
                    if(pattern1_count == 0)
						SFXSound.PlaySFX(7000);

					Instantiate(rock, pattern.transform.position + (pattern.transform.up * 50f), Quaternion.identity);
                    pattern1_count++;
                    //anim.SetBool("doThrow", false);
                }
            }
            //패턴 2
            else if (patternValue == 1)
            {
                pattern.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-50f, 50f)));
                //패턴 색 알파값 증가시키는 반복문
                while (pattern2_duration <= 1f)
                {
                    pattern.transform.position = new Vector3(player.transform.position.x,
                                                                player.transform.position.y,
                                                                pattern.transform.position.z);
                    pattern2_time += Time.deltaTime;
                    pattern2_duration = pattern2_time / pattern2_launchTime;
                    patternColor.material.color = new Color(1, 0, 0, Mathf.Lerp(0f, 0.8f, pattern2_duration));
                    yield return null;
                }
                anim.SetBool("doJump", true);
                yield return new WaitForSeconds(1f);
                pattern.SetActive(false);
                anim.SetBool("doJump", false);
                tiger.SetActive(false);
                yield return new WaitForSeconds(0.3f);
                // 호랑이 Claw 사운드
                SFXSound.PlaySFX(16000);
                claw.transform.position = new Vector3(pattern.transform.position.x,
                                                        pattern.transform.position.y,
                                                        claw.transform.position.z);
                claw.transform.rotation = pattern.transform.rotation;
                claw.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                claw.SetActive(false);
                tiger.SetActive(true);
                tiger.transform.localPosition = new Vector3(0, 2.07f, 0);
            }
            pattern.SetActive(false);

            pattern.transform.parent = claw.transform.parent = gameObject.transform;

            yield return new WaitForSeconds(patternCooldown);
        }
    }
}