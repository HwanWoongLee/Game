using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour
{
    /// <summary>
    /// 기존 카메라(SmoothCamera)로 좌표비교하여 Drag를하면 
    /// 카메라 움직임에 따라 좌표가 정확하지 않아 subCam으로 Drag 보조
    /// </summary>
    [SerializeField]
    private Camera subCam;

    [SerializeField]
    private SmoothCamera cam,bloomCam;

    [SerializeField]
    private GameObject player;

    public GameObject joyStick;
    public GameObject dirStick;

    private Vector3 joyDragVec, dirDragVec;        //마우스 드래그 위치
    private Vector3 startVec, startVec2;
    private Vector3 calPos, calPos2;         //차이 벡터

    private Vector3 temp;


    //드래그 상태
    public bool joyDrag, dirDrag;

    public float distance = 0f;
    public float limitDistance = 20f;

    int joyNum, dirNum;

    [SerializeField]
    private GameObject[] standard;

    private void Update()
    {
        if (GameManager.instance.curGameState == GameState.store
            || GameManager.instance.curGameState == GameState.store2)
            return;
        
        cam.ZoomCamera(joyDrag);
        bloomCam.ZoomCamera(joyDrag);

        OnTouch();
        if (GameManager.instance.curGameState == GameState.game 
            || GameManager.instance.curGameState == GameState.ready)
        {
            PlayerMove();
        }
    }

    private void Start()
    {
        startVec = joyStick.transform.position;
        startVec2 = dirStick.transform.position;
        temp = Vector3.up;

        joyNum = -1;
        dirNum = -1;
    }

    //게임상태 터치조작
    private void OnTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch[] _touch = Input.touches;

            for (int i = 0; i < Input.touchCount; i++)
            {
                //터치시작
                if (_touch[i].phase.Equals(TouchPhase.Began))
                {
                    if (GameManager.instance.curGameState == GameState.main)
                    {
                        Ray ray = subCam.ScreenPointToRay(_touch[i].position);

                        RaycastHit hit;

                        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
                        {
                            //버튼 말고 배경을 누르면 시작
                            if (!hit.collider.tag.Equals("Button"))
                            {
                                GameManager.instance.StateTransition(GameState.game);
                                SoundManager.instance.PlayEffectSound(0);
                            }
                        }
                    }
                    else if (GameManager.instance.curGameState == GameState.game
                        || GameManager.instance.curGameState == GameState.ready)
                    {
                        Ray ray = subCam.ScreenPointToRay(_touch[i].position);

                        RaycastHit hit;

                        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
                        {
                            if (hit.collider.tag.Equals("JoyStick"))
                            {
                                standard[0].SetActive(true);
                                joyNum = i;
                                joyDrag = true;
                            }
                            else if (hit.collider.tag.Equals("DirectionStick"))
                            {
                                standard[1].SetActive(true);
                                dirNum = i;

                                dirDrag = true;
                            }
                        }
                    }
                }

                //드래그
                if (_touch[i].phase.Equals(TouchPhase.Moved))
                {
                    if (joyDrag && i == joyNum)
                    {
                        joyDragVec = subCam.ScreenToWorldPoint(_touch[i].position);
                    }
                    else if (dirDrag && i == dirNum)
                    {
                        dirDragVec = subCam.ScreenToWorldPoint(_touch[i].position);
                    }
                }

                //터치 뗄때
                if (_touch[i].phase.Equals(TouchPhase.Ended))
                {
                    if (i == joyNum && joyDrag)
                    {
                        standard[0].SetActive(false);
                        joyDrag = false;
                        joyDragVec = startVec;
                        joyStick.transform.position = startVec;
                        joyNum = -1;
                        dirNum = 0;
                    }
                    else if (i == dirNum && dirDrag)
                    {
                        standard[1].SetActive(false);
                        dirDrag = false;
                        dirDragVec = startVec2;
                        dirStick.transform.position = startVec2;
                        dirNum = -1;
                        joyNum = 0;
                    }
                }
            }
        }
        else if (Input.touchCount == 0)
        {
            joyDrag = false;
            joyDragVec = startVec;
            joyStick.transform.position = startVec;
            dirDrag = false;
            dirDragVec = startVec2;
            dirStick.transform.position = startVec2;
        }
    }

    private void PlayerMove()
    {
        //이동 조이스틱 드래그 상태일때
        if (joyDrag)
        {
            //드래그 길이 체크.
            distance = Vector2.Distance(startVec, joyDragVec);

            //드래그 표시
            if (distance >= 2)
            {
                distance = 2;
                joyStick.transform.position = joyDragVec + Vector3.forward * 9;
            }
            else
            {
                joyStick.transform.position = joyDragVec + Vector3.forward * 9;
            }
            //드래그 거리만큼 시간조정
            GameManager.instance.SetTimeScale(distance * 0.5f);

            calPos = joyDragVec - startVec;

            calPos = calPos.normalized;

            //calPos 방향으로 moveSpeed로 이동
            Vector2 movePos = calPos * Time.deltaTime * player.GetComponent<Player>().moveSpeed;
            player.transform.Translate(movePos, Space.World);

            //이동제한.
            player.transform.position = Vector3.ClampMagnitude(player.transform.position, limitDistance);
        }
        else
        {
            GameManager.instance.SetTimeScale(0f);

            distance = 0;
        }

        if (dirDrag)
        {
            dirStick.transform.position = dirDragVec + Vector3.forward * 9;

            calPos2 = dirDragVec - startVec2;
            calPos2 = calPos2.normalized;

            if (calPos2 != Vector3.zero)
            {
                temp = calPos2;
            }

            player.GetComponent<Player>().SetPlayerDirection(temp);
        }
    }
}