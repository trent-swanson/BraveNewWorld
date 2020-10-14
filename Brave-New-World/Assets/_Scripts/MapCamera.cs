using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour {
    static MapCamera instance;
	Transform swivel, stick;
    float zoom = 1f;
    float rotationAngle;
    Vector3 dragOrigin;

    public float stickMinZoom, stickMaxZoom;
    public float swivelMinZoom, swivelMaxZoom;
    public float moveSpeedMinZoom, moveSpeedMaxZoom;
    public float dragSpeedMinZoom, dragSpeedMaxZoom;
    public float rotationSpeed;

    //public HexGrid grid;

	void Awake () {
        instance = this;
		swivel = transform.GetChild(0);
		stick = swivel.GetChild(0);
	}

    public static bool Locked {
		set {
			instance.enabled = !value;
		}
	}

	public static void ValidatePosition () {
		instance.AdjustPosition(0f, 0f);
	}

    void LateUpdate () {
		//Zooming
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
		if (zoomDelta != 0f) {
			AdjustZoom(zoomDelta);
		}

        //Rotating
        float rotationDelta = Input.GetAxis("Rotation");
		if (rotationDelta != 0f) {
			AdjustRotation(rotationDelta);
		}

        //Moving
        float xDelta = Input.GetAxis("Horizontal");
		float zDelta = Input.GetAxis("Vertical");
		if (xDelta != 0f || zDelta != 0f) {
			AdjustPosition(xDelta, zDelta);
		}

        //Dragging
        DragPosition();
	}
	
	void AdjustZoom (float delta) {
        zoom = Mathf.Clamp01(zoom + delta);

        float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
		stick.localPosition = new Vector3(0f, 0f, distance);

        float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
		swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
	}

    void AdjustRotation (float delta) {
		rotationAngle += delta * rotationSpeed * Time.deltaTime;
        if (rotationAngle < 0f) {
			rotationAngle += 360f;
		}
		else if (rotationAngle >= 360f) {
			rotationAngle -= 360f;
		}
		transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
	}

    void AdjustPosition (float xDelta, float zDelta) {
        Vector3 direction = transform.localRotation *
        new Vector3(xDelta, 0f, zDelta).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
		float distance = Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom)
        * damping * Time.deltaTime;

		Vector3 position = transform.localPosition;
		position += direction * distance;
        transform.localPosition = position;
		//transform.localPosition = ClampPosition(position);
	}

    /*Vector3 ClampPosition (Vector3 position) {
		float xMax = (grid.cellCountX - 0.5f)
			* (2f * HexMetrics.innerRadius);
		position.x = Mathf.Clamp(position.x, 0f, xMax);

        float zMax = (grid.cellCountZ - 1)
			* (1.5f * HexMetrics.outerRadius);
		position.z = Mathf.Clamp(position.z, 0f, zMax);

		return position;
	}*/

    void DragPosition() {
        if (Input.GetMouseButtonDown(2)) {
            dragOrigin = Input.mousePosition;
        }
        if (Input.GetMouseButton(2)) {
            Vector3 dragDelta = transform.localRotation * new Vector3(dragOrigin.x - Input.mousePosition.x, 0f, dragOrigin.y - Input.mousePosition.y);
            float distance = Mathf.Lerp(dragSpeedMinZoom, dragSpeedMaxZoom, zoom) * Time.deltaTime;

            Vector3 position = transform.localPosition;
            position += dragDelta * distance;
            transform.localPosition = position;
            //transform.localPosition = ClampPosition(position);
            dragOrigin = Input.mousePosition;
        }
    }
}
