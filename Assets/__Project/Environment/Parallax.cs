using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Parallax : MonoBehaviour {
    private const float BUFFER_AREA_SIZE = 1.2f;

    [SerializeField]
    private float _parallaxEffect = 1;

    [SerializeField]
    private Camera _camera;

    private bool isSecondary = false;
    private Vector3 _cameraStartPosition;
    private Vector3 _spriteStartPosition;
    private SpriteRenderer _spriteRenderer;
    private float _cameraHalfWidth;
    private float _spriteWidth;
    private Parallax[] _frames;
    private int _mostLeftFrameIndex = 0;
    private int _mostRightFrameIndex {
        get => (_mostLeftFrameIndex - 1 + _frames.Length) % _frames.Length;
        set => _mostLeftFrameIndex = (value + 1) % _frames.Length;
    }

    private float _rightVisibleBorder;
    private float _rightSpritesBorder;

    void Start() {
        if (_camera == null) {
            if (Camera.main == null) {
                Debug.LogError($"Attach camera or add default camera to the scene!", this);
            }
            else {
                _camera = Camera.main;
            }
        }
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _cameraStartPosition = _camera.transform.position;
        _spriteStartPosition = transform.position;

        _cameraHalfWidth = _camera.orthographicSize * _camera.aspect;
        _spriteWidth = _spriteRenderer.bounds.size.x;

        if (!isSecondary) {
            _frames = InitializeFrames(_cameraHalfWidth * 2, _spriteWidth);
        }
    }

    void Update() {
        RefreshPosition();
    }

    void LateUpdate() {
        if (!isSecondary) {
            CheckFramesVisibility();
        }
    }

    void CheckFramesVisibility() {
        var cameraPosition = _camera.transform.position;
        var cameraHalfWidthBufferred = _cameraHalfWidth * BUFFER_AREA_SIZE;
        var spriteHalfWidth = _spriteWidth / 2;

        float leftVisibleBorder = cameraPosition.x - cameraHalfWidthBufferred;
        Parallax mostLeftFrame = _frames[_mostLeftFrameIndex];
        float leftSpritesBorder = mostLeftFrame.transform.position.x - spriteHalfWidth;

        float rightVisibleBorder = _rightVisibleBorder = cameraPosition.x + cameraHalfWidthBufferred;
        Parallax mostRightFrame = _frames[_mostRightFrameIndex];
        float rightSpritesBorder = _rightSpritesBorder = mostRightFrame.transform.position.x + spriteHalfWidth;

        if (leftVisibleBorder < leftSpritesBorder) {
            ShiftFrame(mostRightFrame, mostLeftFrame.transform.position.x - _spriteWidth);

            // Move index pointer to new most left frame
            _mostLeftFrameIndex = _mostRightFrameIndex;
        }
        else if (rightVisibleBorder > rightSpritesBorder) {
            ShiftFrame(mostLeftFrame, mostRightFrame.transform.position.x + _spriteWidth);

            // Move index pointer to new most right frame
            _mostRightFrameIndex = _mostLeftFrameIndex;
        }
    }

    void ShiftFrame(Parallax frame, float newPositionX) {
        var framePosition = frame.transform.position;
        framePosition.x = newPositionX;
        frame.transform.position = framePosition;

        float cameraMovedDistanceX = _camera.transform.position.x - _cameraStartPosition.x;

        var spriteStartPosition = frame._spriteStartPosition;
        spriteStartPosition.x = newPositionX - cameraMovedDistanceX * _parallaxEffect;
        frame._spriteStartPosition = spriteStartPosition;
    }

    int GetFramesNeededNumber(float cameraWidth, float spriteWidth) {
        float cameraWidthWithBuffer = cameraWidth * BUFFER_AREA_SIZE;

        // Floor, because we need to cover the most area and in any case we will add 2 more frames later
        int spritesNumberToCoverMostArea = Mathf.FloorToInt(cameraWidthWithBuffer / spriteWidth);

        // Add 2 sprites for both sides
        return spritesNumberToCoverMostArea + 2;
    }

    Parallax[] InitializeFrames(float cameraWidth, float spriteWidth) {
        Transform holder = new GameObject($"{name} holder").transform;
        holder.parent = transform.parent;

        string originalName = name;

        int framesNumber = GetFramesNeededNumber(cameraWidth, spriteWidth);
        var frames = new Parallax[framesNumber];

        var index = 0;
        Vector3 position = transform.position;
        while (index < framesNumber) {
            GameObject frame;
            bool isSecondary = index != 0;
            if (isSecondary) {
                position.x += spriteWidth;
                frame = Instantiate(gameObject, position, Quaternion.identity);
            }
            else {
                frame = gameObject;
            }

            frame.transform.parent = holder;
            frame.name = $"{originalName} {index}";

            var frameParallax = frame.GetComponent<Parallax>();
            frameParallax.isSecondary = isSecondary;

            frames[index] = frameParallax;
            index++;
        }

        return frames;
    }

    void RefreshPosition() {
        Vector3 cameraMovedDistance = _camera.transform.position - _cameraStartPosition;

        transform.position = new Vector3(
            _spriteStartPosition.x + cameraMovedDistance.x * _parallaxEffect,
            _spriteStartPosition.y + cameraMovedDistance.y,
            _spriteStartPosition.z
        );
    }
}
