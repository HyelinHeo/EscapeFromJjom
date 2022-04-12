using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float rot_speed = 100.0f;

    public Transform MainCamera;

    private float camera_dist = 0f; //리그로부터 카메라까지의 거리
    public float camera_width = -2; //가로거리
    public float camera_height = 2f; //세로거리
    public float camera_fix = 3f;//레이케스트 후 리그쪽으로 올 거리

    Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        //카메라리그에서 카메라까지의 길이
        camera_dist = Mathf.Sqrt(camera_width * camera_width + camera_height * camera_height);

        //카메라리그에서 카메라위치까지의 방향벡터
        dir = new Vector3(0, camera_height, camera_width).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        //레이캐스트할 벡터값
        Vector3 ray_target = transform.up * camera_height + transform.forward * camera_width;

        RaycastHit hitinfo;
        Physics.Raycast(transform.position, ray_target, out hitinfo, camera_dist);

        if (hitinfo.point != Vector3.zero)//레이케스트 성공시
        {
            //point로 옮긴다.
            MainCamera.position = hitinfo.point;
            //카메라 보정
            MainCamera.Translate(dir * -1 * camera_fix);
        }
        else
        {
            //로컬좌표를 0으로 맞춘다. (카메라리그로 옮긴다.)
            MainCamera.localPosition = Vector3.zero;
            //카메라위치까지의 방향벡터 * 카메라 최대거리 로 옮긴다.
            MainCamera.Translate(dir * camera_dist);
            //카메라 보정
            MainCamera.Translate(dir * -1 * camera_fix);

        }
    }
}
