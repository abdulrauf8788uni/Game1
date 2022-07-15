using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 10f;
    public float dragSpeed = 10f;  
    public float maxSpeed = 100f;
    
    private Rigidbody rb;
    private float moveX;
    private float moveY;
    private float totalSpeed;
    private Vector3 originalSize;

    // For powerups
    [Space] // Id: 1, Increase Speed to maxSpeed
    public float speedMultiplier = 1.3f;
    public float speedMulDuration = 3f; 
    private bool speedMulActive;
    private float speedBoostTimer;

    [Space] // Id: 2, Increase size my size Multiplier
    public float sizeMultiplier = 10f;
    public float sizeMulDuration = 3f;
    private bool sizeMulActive;
    private float sizeMulTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalSize = transform.localScale;

        ResetPowerUpStats(); // Reset the PowerUp status to false
    }

    void OnMove(InputValue moveVal) {
        Vector2 moveVector = moveVal.Get<Vector2>();

        moveX = moveVector.x;
        moveY = moveVector.y;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPowerUps();
    }
    void FixedUpdate() {
        Vector3 movement = (new Vector3(moveX, 0, moveY) * dragSpeed); // Left and Right movement
        if (rb.velocity.z <= maxSpeed){
            movement += (Vector3.forward *  totalSpeed); // If not max Speed then add forward force. 
        }
        rb.AddForce(movement, ForceMode.Acceleration);
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.CompareTag("PowerUp")) {
            int powerUpId = col.gameObject.GetComponent<PowerUpScript>().PowerUpId;
            Destroy(col.gameObject);
            ActivatePowerUp(powerUpId);
        }
    }

    void ResetPowerUpStats () {
        ResetSpeedUpEffect();
        ResetSizeMulEffect();
    }

    void ResetSpeedUpEffect(){
        speedMulActive = false;
        speedBoostTimer = 0f;
        totalSpeed = forwardSpeed;
    }

    void ResetSizeMulEffect() {
        sizeMulActive = false;
        sizeMulTimer = 0f;
        transform.localScale = originalSize;
    }

    void ApplySpeedMulEffect(){
        speedMulActive = true;
        totalSpeed = maxSpeed;
    }

    void ApplySizeMulEffect() {
        sizeMulActive = true;
        transform.localScale = transform.localScale * sizeMultiplier;
    }

    void CheckForPowerUps(){ // Check and apply picked powerups until time
        if (speedMulActive) {
            speedBoostTimer += Time.deltaTime;
            if (speedBoostTimer >= speedMulDuration){
                ResetSpeedUpEffect();
            }
        }
        if (sizeMulActive){
            sizeMulTimer += Time.deltaTime;
            if (sizeMulTimer >= sizeMulDuration){
                ResetSizeMulEffect();
            }
        }
    }
    void ActivatePowerUp(int powerUpId){ // Activate PowerUp With PowerUpId
        if (powerUpId == 1){ // 1 = Speed Boost
            ApplySpeedMulEffect();
        }
        else if (powerUpId == 2){
            ApplySizeMulEffect();
        }
    }
}
