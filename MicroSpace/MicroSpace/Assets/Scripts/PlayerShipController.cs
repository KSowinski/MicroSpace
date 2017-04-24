using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShipController : MonoBehaviour, IInit
{
    public GameObject MarkerCursorPrefab;
    public GameObject MovingParticles;
    public GameObject IdleParticles;
    public GameObject RotationParticlesLeft;
    public GameObject RotationParticlesRight;
    public GameObject DamageEffect;
    public GameObject LaserPrefab;

    public GameObject[] Guns;
    public GameObject[] FirePoints;

    private Vector3 _mousePosition;
    private float _moveProgress = 0f;
    private float _angle;
    private bool _newPoint = false;
    private Vector3 _startPosition;
    private GameObject _marker;
    private float _previousAngle;
    private float _damageDelay = 0.5f;
    private float _fireDelay = 0f;
    private bool _teleporting = false;

    public void Init()
    {

    }

    void Update ()
    {
        //Teleporting
        if (_teleporting)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * 500f);
            transform.localScale = new Vector3(transform.localScale.x - 0.001f, transform.localScale.y - 0.001f, 1f);

            if (transform.localScale.x <= 0f)
            {
                var whOut = GameObject.FindGameObjectWithTag("WormholeOut").transform.position;
                transform.position = new Vector3(whOut.x, whOut.y, 0f);
                transform.localScale = new Vector3(0.15f, 0.15f, 1f);
                Inpt.IsEnabled = true;
                IdleParticles.SetActive(true);
                _teleporting = false;
            }
            else
            {
                return;
            }
        }


        //Keyboard shortcuts
        KeybordShortcuts();

        //Move ship even if the bridge is not selected
        if(ShipController.Fuel.Current > 0) MoveShip();

        //Update damage delay
        UpdateDamageDelay();

        //Response to input based on curently active ship area
        if (ShipController.SelectedShipArea == SelectedShipArea.Bridge)
        {
            if (ShipController.Fuel.Current > 0) ShipMovementControl();
        }

        //Wepons
        if (ShipController.SelectedShipArea == SelectedShipArea.Weapons)
        {
            LookAtMouseCursor();
            Fire();
        }
        
    }

    private void LookAtMouseCursor()
    {
        var mouseScreenPosition = (Vector2)Camera.main.ScreenToWorldPoint(Inpt.MousePosition());
        foreach (var gun in Guns)
        {
            var direction = (mouseScreenPosition - (Vector2)gun.transform.position).normalized;
            gun.transform.right = direction;
        }
    }

    private void Fire()
    {
        if (_fireDelay > 0f)
        {
            _fireDelay -= Time.deltaTime;
            return;
        }

        if (Inpt.GetMouseButton(0))
        {
            var index = Random.Range(0, FirePoints.Length);
            var rndFirePoint = FirePoints[index];
            var rndGun = Guns[index];
            var laser = Instantiate(LaserPrefab, rndFirePoint.transform.position, rndGun.transform.rotation);
            var x = GetLaserColor();
            laser.GetComponent<SpriteRenderer>().color = x;
            laser.name = "PlayerBullet";
            _fireDelay = 0.15f;
            SoundManager.Inst().Laser();
        }
    }

    private void UpdateDamageDelay()
    {
        if (_damageDelay > 0f)
        {
            _damageDelay -= Time.deltaTime;
            if (_damageDelay < 0f) _damageDelay = 0f;
        }
    }

    #region KEYBORAD SHORTCUTS

    private static void KeybordShortcuts()
    {
        if (Inpt.GetKeyDown(KeyCode.Alpha1))
        {
            FindObjectsOfType<ShipAreaController>().First(x => x.OnSelectionArea == SelectedShipArea.Bridge).SetActive();
        }
        else if (Inpt.GetKeyDown(KeyCode.Alpha2))
        {
            FindObjectsOfType<ShipAreaController>().First(x => x.OnSelectionArea == SelectedShipArea.Comms).SetActive();
        }
        else if (Inpt.GetKeyDown(KeyCode.Alpha3))
        {
            FindObjectsOfType<ShipAreaController>().First(x => x.OnSelectionArea == SelectedShipArea.Beam).SetActive();
        }
        else if (Inpt.GetKeyDown(KeyCode.Alpha4))
        {
            FindObjectsOfType<ShipAreaController>().First(x => x.OnSelectionArea == SelectedShipArea.Shield).SetActive();
        }
        else if (Inpt.GetKeyDown(KeyCode.Alpha5))
        {
            FindObjectsOfType<ShipAreaController>().First(x => x.OnSelectionArea == SelectedShipArea.Weapons).SetActive();
        }
        else if (Inpt.GetKeyDown(KeyCode.Alpha6))
        {
            FindObjectsOfType<ShipAreaController>().First(x => x.OnSelectionArea == SelectedShipArea.Repair).SetActive();
        }
    }

    #endregion

    #region SHIP MOVEMENT

    private void ShipMovementControl()
    {
        if (Inpt.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Inpt.MousePosition());
            _mousePosition = new Vector3(_mousePosition.x, _mousePosition.y, 0f);
            _angle = (Mathf.Atan2(_mousePosition.y - transform.position.y, _mousePosition.x - transform.position.x)*
                      Mathf.Rad2Deg);
            _angle = (_angle < 0f) ? _angle + 360f : _angle;
            _startPosition = transform.position;
            _moveProgress = 0f;
            _newPoint = true;
            _previousAngle = transform.eulerAngles.z;

            //Place cursor
            if (_marker == null)
            {
                _marker = GameObject.FindGameObjectWithTag("MarkerCursor") ??
                          Instantiate(MarkerCursorPrefab, _mousePosition, Quaternion.identity);
            }
            _marker.transform.position = _mousePosition;
            _marker.SetActive(true);

        }
    }

    private void MoveShip()
    {
        if (!_newPoint) return;

        //If rotation of ship is correct
        var isAngleSet = false;
        if (_angle < 0f && transform.eulerAngles.z >= _angle - 1f && transform.eulerAngles.z <= _angle + 1f)
        {
            isAngleSet = true;
        }
        else if (_angle >= 0f && transform.eulerAngles.z <= _angle + 1f && transform.eulerAngles.z >= _angle - 1f)
        {
            isAngleSet = true;
        }

        //ROTATE
        if (!isAngleSet)
        {
            var rotationSpeed = 2.25f + ShipController.Thrusters.Current*0.25f;
            var quaternion = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, _angle), rotationSpeed * Time.deltaTime);
            transform.rotation = quaternion;

            //Side thrusters
            MovingParticles.SetActive(false);
            if (_previousAngle < transform.eulerAngles.z)
            {
                RotationParticlesLeft.SetActive(false);
                RotationParticlesRight.SetActive(true);
            }
            else if (_previousAngle > transform.eulerAngles.z)
            {
                RotationParticlesLeft.SetActive(true);
                RotationParticlesRight.SetActive(false);
            }
            else
            {
                RotationParticlesLeft.SetActive(false);
                RotationParticlesRight.SetActive(false);
            }
            _previousAngle = transform.eulerAngles.z;

            return;
        }

        //MOVE
        RotationParticlesLeft.SetActive(false);
        RotationParticlesRight.SetActive(false);
        var dist = Vector2.Distance(transform.position, _mousePosition);
        if (dist > 0.25f)
        {
            _moveProgress += Time.deltaTime * (0.1f + ShipController.Thrusters.Current * 0.05f);
            var position = Vector2.Lerp(_startPosition, _mousePosition, _moveProgress);
            transform.position = position;
            MovingParticles.SetActive(true);
            ConsumeFuel();
        }
        else
        {
            _newPoint = false;
            _marker.SetActive(false);
            MovingParticles.SetActive(false);
        }
    }

    private float _fuelConsumptionDelay = 0.5f;
    private void ConsumeFuel()
    {
        if (_fuelConsumptionDelay > 0f)
        {
            _fuelConsumptionDelay -= Time.deltaTime;
            return;
        }

        ShipController.Fuel.Update(-1, null);
        _fuelConsumptionDelay = 0.1f;

        if (ShipController.Fuel.Current <= 0)
        {
            MovingParticles.SetActive(false);
            RotationParticlesLeft.SetActive(false);
            RotationParticlesRight.SetActive(false);
            GameObject.FindGameObjectWithTag("MarkerCursor").SetActive(false);
            _newPoint = false;
        }
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(_damageDelay > 0f) return;

        if (coll.gameObject.name.Contains("Asteroid") || coll.gameObject.name.Contains("Moon"))
        {
            Instantiate(DamageEffect, coll.contacts.First().point, Quaternion.identity);
            _damageDelay = 0.25f;
            DoDamage(10);
        }
        else if (coll.collider.name.Contains("EnemyBullet"))
        {
            Instantiate(DamageEffect, coll.contacts.First().point, Quaternion.identity);
            _damageDelay = 0.25f;
            DoDamage(10);
            Destroy(coll.collider);
        }
    }

    private void DoDamage(int value)
    {
        SoundManager.Inst().Damage();

        //Update shield/hull
        if (ShipController.Shield.Current >= value)
        {
            ShipController.Shield.Update(-1 * value, null);
        }
        else if (ShipController.Shield.Current < value)
        {
            if (ShipController.Shield.Current > 0)
            {
                value -= ShipController.Shield.Current;
                ShipController.Shield.Update(-1 * ShipController.Shield.Current, null);
            }

            ShipController.Hull.Update(-1 * value, null);

            if (ShipController.Hull.Current <= 0)
            {
                _newPoint = false;
                GameOverController.ShhhOnlyDreamsNow();
            }
        }
    }

    private static Color GetLaserColor()
    {
        switch (ShipController.Weapons.Current)
        {
            case 1: return      new Color(22 / 255f, 234 / 255f, 128 / 255f);
            case 2: return      new Color(89 / 255f, 22 / 255f, 234 / 255f);
            case 3: return      new Color(234 / 255f, 195 / 255f, 22 / 255f);
            case 4: return      new Color(195 / 255f, 234 / 255f, 128 / 255f);
            case 5: return      new Color(219 / 255f, 19 / 255f, 206 / 255f);
            case 6: return      new Color(5 / 255f, 13 / 255f, 245 / 255f);
            case 7: return      new Color(164 / 255f, 214 / 255f, 136 / 255f);
            case 8: return      new Color(17 / 255f, 201 / 255f, 238 / 255f);
            case 9: return      new Color(255 / 255f, 0f, 0f);
            default: return     new Color(255 / 255f, 255 / 255f, 255 / 255f);
        }
    }

    public bool IsTeleporting()
    {
        return _teleporting;
    }

    public void UseWormhole()
    {
        IdleParticles.SetActive(false);
        MovingParticles.SetActive(false);
        RotationParticlesLeft.SetActive(false);
        RotationParticlesRight.SetActive(false);
        GameObject.FindGameObjectWithTag("MarkerCursor").SetActive(false);
        _newPoint = false;
        Inpt.IsEnabled = false;
        _teleporting = true;
    }
}
