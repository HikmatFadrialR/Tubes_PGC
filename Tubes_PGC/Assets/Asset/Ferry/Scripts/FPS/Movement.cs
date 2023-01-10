using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveMode {
    Walk, Crouch, Run
}

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [Header("Move")]
    public float gravity; //gravitasi
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public MoveMode moveMode;

    [Header("Mouse")]

    public float minVerticalLook; //Minimal batas rotasi camera secara vertical
    public float maxVerticalLook; //Maximal batas rotasi camera secara vertical
    public float sensitivty; //sensitivas mouse

    [Header("Crouch")]
    public float crouchHeight; 
    public float normalHeight;
    public Transform CameraHolder; 
    public LayerMask crouchLayer; //layer untuk benda-benda untuk deteksi crouch

    private float deltaX, deltaZ, rotationX, rotationY, speed, newHeight;
    private CharacterController charController;
    private Vector3 movement;

    private void Start () {
        Cursor.visible = false; //menghilangkan cursor atau menyembunyikannya
        Cursor.lockState = CursorLockMode.Locked; //mengkunci cursor agar tidak keluar dari game
        charController = GetComponent<CharacterController>(); 
    }

    private void Update () {
        Move();
        Look();
        SpeedControl();
    }

    private void SpeedControl () {
        bool isRun = Input.GetKey(KeyCode.LeftShift) && deltaZ > 1f && charController.isGrounded; //untuk lari player harus berjalan terlebih dahulu kemudian player harus menginjak tanah
        bool isCrouch = Input.GetKey(KeyCode.C) && isRun == false; //player bisa menunduk ketika player tidak berlari

        if(isRun) {
            moveMode = MoveMode.Run;
        } else if(isCrouch || CheckCrouch()) {
            moveMode = MoveMode.Crouch;
        } else {
            moveMode = MoveMode.Walk;
        }

        //dibawah ini adalah transisi smooth antara perubahan nilai speed
        switch(moveMode) {
            case MoveMode.Crouch : 
                if(speed != crouchSpeed)
                    speed = Mathf.Lerp(speed, crouchSpeed, 7 * Time.deltaTime);
            break;
            case MoveMode.Run : 
                if(speed != runSpeed)
                    speed = Mathf.Lerp(speed, runSpeed, 7 * Time.deltaTime);
            break;
            case MoveMode.Walk : 
                if(speed != walkSpeed)
                    speed = Mathf.Lerp(speed, walkSpeed, 7 * Time.deltaTime);
            break;
        }

        if(isCrouch || CheckCrouch()) {
            newHeight = crouchHeight; //new height menjadi crouch height ketika player menekan tombol crouch
        } else {
            newHeight = normalHeight; //new height menjadi normal height ketika player melepas tombol crouch
        }
        if(charController.height != newHeight) charController.height = Mathf.MoveTowards(charController.height, newHeight, 5 * Time.deltaTime); //transisi mengubah char.height menjadi newHeight
        charController.center = new Vector3(charController.center.x, charController.height * 0.5f, charController.center.z); //menyesuaikan posisi player ketika melakukan crouch 
        CameraHolder.transform.localPosition = new Vector3(CameraHolder.localPosition.x, charController.height + 0.3f, CameraHolder.localPosition.z); //menyesuaikan posisi camera agar camera tersebut sesuai dengan posisi crouchnya
    }

    private void Move() {
        deltaX = Input.GetAxis("Horizontal") * speed; //Inputan player bergerak secara horizontal (kekiri dan kekanan)
        deltaZ = Input.GetAxis("Vertical") * speed; //Inputan player bergerak secara forward atau backward (kedepan dan kebelakang)
        movement = new Vector3 (deltaX, 0, deltaZ);
	    movement = Vector3.ClampMagnitude (movement, speed); //mengatur kecepatan bergerak secara diagonal, jika code ini dihilangkan maka bergerak secara diagonal akan lebih cepat dari bergerak dengan kecepatan yang kita tentukan
        movement.y = -gravity; //gravitasi player
        movement.y = AdjustVelocityToSlope(movement).y; //untuk check apakah ground yang di injak itu menanjak atau sebaliknya 
	    movement *= Time.deltaTime; //timedeltaTime agar gerakan player tidak kecepatan
	    movement = transform.TransformDirection (movement); 
        charController.Move (movement);
    }

    private void Look () {
        float mouseX = Input.GetAxis("Mouse X") * sensitivty; //Inputan player melihat secara horizontal
        float mouseY = Input.GetAxis("Mouse Y") * sensitivty; //Inputan player melihat secara vertical

        rotationX += mouseX; 
        rotationY -= mouseY; 

        rotationY = Mathf.Clamp(rotationY, minVerticalLook, maxVerticalLook); //membatasi rotasi vertical camera
        if(mouseX != 0)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationX, transform.eulerAngles.z);
            //rotasi horizontal hanya di bagian badan atau bagian objek player nya ini
        if(mouseY != 0)
            CameraHolder.localEulerAngles = new Vector3(rotationY, CameraHolder.localEulerAngles.y, CameraHolder.localEulerAngles.z);
            //rotasi vertical hanya di bagian kepalanya saja, kepala atau camera player
    }

    Vector3[] newVectorCheckCrouch;
    private bool CheckCrouch () {
        //dibawah ini adalah raycast yang posisinya di bagian kepala player, fungsinya ini untuk mengecek apakah diatas player ada benda atau tidak
        //jika ada benda maka player akan crouching
        float checkRayAbove = charController.bounds.center.y + charController.bounds.extents.y;  //memposisikan raycast di kepala player
        float checkRayFront = charController.bounds.center.z + charController.bounds.extents.z; //memposisikan raycast di kepala player bagian depannya
        float checkRayRight = charController.bounds.center.x + charController.bounds.extents.x; //memposisikan raycast di kepala player bagian kanan atau samping kanan
        float checkRayLeft = charController.bounds.center.x - charController.bounds.extents.x; ////memposisikan raycast di kepala player bagian kiri atau samping kiri
        float checkRayBackward = charController.bounds.center.z - charController.bounds.extents.z; //memposisikan raycast di kepala player bagian belakang
        newVectorCheckCrouch = new Vector3[]{
            new Vector3(transform.position.x, checkRayAbove, checkRayFront), 
            new Vector3(checkRayRight, checkRayAbove, transform.position.z),
            new Vector3(checkRayLeft, checkRayAbove, transform.position.z),
            new Vector3(transform.position.x, checkRayAbove, checkRayBackward)
        };
        bool CheckFront = RayGenerator(newVectorCheckCrouch[0]); //raycast cek
        bool checkRight = RayGenerator(newVectorCheckCrouch[1]);
        bool CheckLeft = RayGenerator(newVectorCheckCrouch[2]);
        bool CheckBackward = RayGenerator(newVectorCheckCrouch[3]);
        return (CheckFront || checkRight || CheckLeft || CheckBackward); //jika salah satu raycast mengenai benda makan checkCrouch == true
    }

    private bool RayGenerator (Vector3 position) {
        RaycastHit hit;
        return Physics.Raycast(position, Vector3.up, out hit, 1, crouchLayer);
        //benda yang akan tercek oleh raycast adalah benda yang memiliki layer crouchLayer
    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity) { //fix bug player melayang ketika berlari kebawah.
        var ray = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f)) {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if(adjustedVelocity.y < 0)
                return adjustedVelocity;
        }

        return velocity; 
    }

    private void OnDrawGizmos () {
        Gizmos.DrawRay(transform.position, AdjustVelocityToSlope(transform.position));
        if(newVectorCheckCrouch != null) {
            for (int i = 0; i < newVectorCheckCrouch.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(newVectorCheckCrouch[i], Vector3.up * 1);
            }
        }
    }
}
