using UnityEngine;
using UnityEngine.InputSystem;
 
public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;

    private Mouse currentMouse;
    private Vector2 lastFrameMaousePos;
 
    public Vector3 cameraRotation = new Vector3();
    Vector3 currentCamRotation = new Vector3();
 
    public float dist = 4.0f;
    Vector3 currentLookAtPos = new Vector3();

    void Start()
    {
        currentMouse = Mouse.current;
        lastFrameMaousePos = GetMousePosition();
    }

    // Update is called once per frame
    void Update()
    {
        // ゲームパッド右スティックからの値を加算する
        Vector2 mouseMovedLastFrame = GetMousePosition() - lastFrameMaousePos;
        cameraRotation += new Vector3(-mouseMovedLastFrame.y, -mouseMovedLastFrame.x, 0) * 2.0f * Time.deltaTime;

        lastFrameMaousePos = GetMousePosition();

        // X軸回転の制限
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, 15 * Mathf.Deg2Rad, 60 * Mathf.Deg2Rad);
 
        // 遅延用の角度との差分をとる
        Vector3 diff = cameraRotation - currentCamRotation;
        currentCamRotation += WrapAngle(diff) * 0.2f;
 
        // 角度からベクトルを計算する
        Vector3 craneVec = new Vector3
        (
            Mathf.Cos(currentCamRotation.x) * Mathf.Cos(currentCamRotation.y),
            Mathf.Sin(currentCamRotation.x),
            Mathf.Cos(currentCamRotation.x) * Mathf.Sin(currentCamRotation.y)
        );
 
        // 注視点の座標
        Vector3 lookAtPos = player.position + new Vector3(0, 1, 0);
 
        currentLookAtPos += (lookAtPos - currentLookAtPos) * 0.2f;
 
        // カメラの座標を更新する
        transform.position = currentLookAtPos + craneVec * dist;
 
        // プレイヤーの座標にカメラを向ける（これは最後にする）
        transform.LookAt(currentLookAtPos);
    }
 
    // 角度を0～360°に収める関数
    private Vector3 WrapAngle(Vector3 vector)
    {
        vector.x %= Mathf.PI * 2;
        vector.y %= Mathf.PI * 2;
        vector.z %= Mathf.PI * 2;
 
        return vector;
    }

    // マウスのポインター座標を返す
    private Vector2 GetMousePosition()
    {
        return new Vector2(currentMouse.position.x.ReadValue(), currentMouse.position.y.ReadValue());
    }
}