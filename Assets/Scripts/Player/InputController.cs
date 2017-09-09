using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class InputController : MonoBehaviour
{
    private Player _player;
    private Gun _gun;
    public Crosshairs crosshairs;

    private Camera _cam;
    private Vector3 _movement;

    private float _gunHeight = 1.08f;
    private Vector3 lookPoint;

    void Start()
    {
        _player = GetComponent<Player>();
        _gun = GetComponent<Gun>();
        _cam = Camera.main;
    }

    void FixedUpdate()
    {
        //Movement
        bool shoot = Input.GetAxis("Fire1") > 0;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveInput = _cam.transform.TransformDirection(moveInput);
        moveInput.y = 0;

        //Rotation
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.up * _gunHeight);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            lookPoint = ray.GetPoint(rayDistance);

            crosshairs.transform.position = lookPoint;

            if ((new Vector2(lookPoint.x, lookPoint.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > 1)
            {
                _gun.Aim(lookPoint);
            }
        }

        _player.Move(moveInput, lookPoint);
        _gun.Shooting(shoot);
    }
}
