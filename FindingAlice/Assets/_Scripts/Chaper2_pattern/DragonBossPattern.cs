using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBossPattern : MonoBehaviour
{
    public GameObject pattern;
    public GameObject rush;

    GameObject player;
    //GameObject showPattern;

    Renderer patternColor;

    //���� ��ߵ� ��Ÿ��
    float patternCooldown = 4f;
    //���� ������ ��� ������ Ȯ��
    bool isPatternPlay = false;

    float startDelay = 2.0f;

    [HideInInspector] public bool checking = false;

    //���� ���� �׷����� �ð�
    float allRushPattern_launchTime = 1f;
    //�ð� ������ �����ϴ� ����
    float allRushPattern_duration;
    float allRushPattern_time;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        patternColor = pattern.GetComponent<Renderer>();
        //showPattern = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x,
                                 this.transform.position.y,
                                 this.transform.position.z);

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!checking)
            {
                PatternPlay();
                checking = true;
            }

        }
        if (Input.GetKeyDown(KeyCode.E))
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
            StartCoroutine(TestRushPattern());
        }
    }

    public void PatternExit()
    {
        if (isPatternPlay)
        {
            StopCoroutine(TestRushPattern());
        }
    }

    IEnumerator TestRushPattern()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            pattern.transform.parent = null;
            pattern.SetActive(true);
            allRushPattern_time = 0;
            allRushPattern_duration = 0;

            //pattern.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-50f, 50f)));

            while (allRushPattern_duration <= 0.5f)
            {
                pattern.transform.position = new Vector3(player.transform.position.x,
                                                            player.transform.position.y,
                                                            pattern.transform.position.z);
                allRushPattern_time += Time.deltaTime;
                allRushPattern_duration = allRushPattern_time / allRushPattern_launchTime;
                patternColor.material.color = new Color(1, 0, 0, Mathf.Lerp(0f, 1.6f, allRushPattern_duration));
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            rush.transform.position = new Vector3(pattern.transform.position.x,
                                            pattern.transform.position.y,
                                            rush.transform.position.z);
            rush.transform.rotation = pattern.transform.rotation;
            pattern.SetActive(false);
            rush.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            rush.SetActive(false);

            pattern.transform.parent = rush.transform.parent = gameObject.transform;

            yield return new WaitForSeconds(patternCooldown);
        }

    }
}