using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class KinectTracking : MonoBehaviour
{
    private KinectSensor kinectSensor;
    private BodyFrameReader bodyFrameReader;
    private Body[] bodies;

    public List<GameObject> playerPrefabs; // �÷��̾� ������ ����Ʈ
    public float moveSpeed = 100f;

    // �÷��̾ �����ϴ� Dictionary
    private Dictionary<ulong, GameObject> trackedPlayers = new Dictionary<ulong, GameObject>(); // �÷��̾� ID�� ������Ʈ�� �����ϴ� Dictionary
    private List<ulong> lostIds = new List<ulong>(); // �������� �ʴ� �÷��̾� ID ����Ʈ (����)
    private int prefabIndex = 0; // ���� ����� �÷��̾� ������ �ε���

    void Start()
    {
        // Kinect ������ �ʱ�ȭ
        kinectSensor = KinectSensor.GetDefault();
        if (kinectSensor != null)
        {
            // BodyFrameReader�� ���ϴ�
            bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();
            if (!kinectSensor.IsOpen)
            {
                kinectSensor.Open(); // ������ ���� ���� ������ ���ϴ�
            }
        }
        else
        {
            UnityEngine.Debug.LogError("No Kinect sensor found.");
        }
    }

    void Update()
    {
        if (bodyFrameReader == null) return; // bodyFrameReader�� �ʱ�ȭ���� ���� ��� ����

        var frame = bodyFrameReader.AcquireLatestFrame();
        if (frame != null)
        {
            if (bodies == null)
            {
                bodies = new Body[kinectSensor.BodyFrameSource.BodyCount]; // bodies �迭�� �ʱ�ȭ
            }

            frame.GetAndRefreshBodyData(bodies);
            UpdateTrackedPlayers(); // ������ �÷��̾� ������Ʈ
            RemoveLostPlayers(); // �������� �ʴ� �÷��̾� ����
            frame.Dispose(); // ��� �� �������� ����
        }
    }

    private void UpdateTrackedPlayers()
    {
        lostIds.Clear(); // �������� �ʴ� ID ����Ʈ�� �ʱ�ȭ

        foreach (var body in bodies)
        {
            if (body.IsTracked)
            {
                if (!trackedPlayers.ContainsKey(body.TrackingId))
                {
                    // ���ο� �÷��̾ ����
                    GameObject player = Instantiate(playerPrefabs[prefabIndex], GetInitialPosition(prefabIndex), Quaternion.identity); // �ʱ� ��ġ�� ȸ������ �����Ͽ� ����
                    trackedPlayers.Add(body.TrackingId, player);

                    prefabIndex = (prefabIndex + 1) % playerPrefabs.Count; // ���� �÷��̾� ������ �ε��� ���
                }
                else
                {
                    // �÷��̾ �̹� �����ϸ� Ȱ��ȭ
                    trackedPlayers[body.TrackingId].SetActive(true);
                }
                UpdatePlayerPosition(body, trackedPlayers[body.TrackingId]); // �÷��̾� ��ġ ������Ʈ
            }
        }
    }

    private void RemoveLostPlayers()
    {
        foreach (var id in trackedPlayers.Keys)
        {
            if (!IsTrackedId(id))
            {
                // �������� �ʴ� �÷��̾� ������Ʈ ��Ȱ��ȭ
                trackedPlayers[id].SetActive(false);
                lostIds.Add(id);
            }
        }

        foreach (var id in lostIds)
        {
            trackedPlayers.Remove(id); // �������� �ʴ� �÷��̾� ����
        }
    }

    private bool IsTrackedId(ulong id)
    {
        foreach (var body in bodies)
        {
            if (body.IsTracked && body.TrackingId == id)
            {
                return true;
            }
        }
        return false;
    }

    private Vector3 GetInitialPosition(int prefabIndex)
    {
        // �� �÷��̾� �����պ��� �ʱ� ��ġ�� ����
        switch (prefabIndex)
        {
            case 0: // ù ��° �÷��̾� ������
                return new Vector3(-36.8f, -2.46f, 0f);
            case 1: // �� ��° �÷��̾� ������
                return new Vector3(30.8f, -2.46f, 0f);
            // �ʿ信 ���� �߰����� �÷��̾� �����տ� ���� �ʱ� ��ġ ����
            default:
                return Vector3.zero;
        }
    }

    private void UpdatePlayerPosition(Body body, GameObject player)
    {
        var spineBasePosition = body.Joints[JointType.SpineBase].Position;
        var unitySpineBasePosition = new Vector3(spineBasePosition.X, spineBasePosition.Y, -spineBasePosition.Z);

        // Kinect �����͸� ������� ���ο� ��ġ ���
        var newPosition = new Vector3(unitySpineBasePosition.x * moveSpeed, player.transform.position.y, player.transform.position.z);

        // �÷��̾� ��ġ ������Ʈ
        player.transform.position = newPosition;
    }

    void OnDestroy()
    {
        // bodyFrameReader�� null�� �ƴ� ��� Dispose
        bodyFrameReader?.Dispose();
        bodyFrameReader = null;

        // Kinect ������ ���� �ִ� ��� �ݱ�
        if (kinectSensor != null && kinectSensor.IsOpen)
        {
            kinectSensor.Close();
        }
        kinectSensor = null;
    }
}
