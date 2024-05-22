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
    private Dictionary<ulong, int> playerIndices = new Dictionary<ulong, int>(); // �÷��̾� ID�� �ε����� �����ϴ� Dictionary
    private List<GameObject> players = new List<GameObject>(); // �÷��̾� ������Ʈ ����Ʈ
    private List<ulong> trackedIds = new List<ulong>(); // ������ �÷��̾� ID ����Ʈ (����)
    private List<ulong> lostIds = new List<ulong>(); // �������� �ʴ� �÷��̾� ID ����Ʈ (����)

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
        trackedIds.Clear(); // ������ ID ����Ʈ�� �ʱ�ȭ

        foreach (var body in bodies)
        {
            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId); // ������ ID ����Ʈ�� �߰�
                if (!playerIndices.ContainsKey(body.TrackingId))
                {
                    // ���ο� �÷��̾ ����
                    int prefabIndex = Random.Range(0, playerPrefabs.Count); // �����ϰ� �÷��̾� �������� ����
                    GameObject player = Instantiate(playerPrefabs[prefabIndex]);
                    playerIndices.Add(body.TrackingId, players.Count);
                    players.Add(player);
                }
                else
                {
                    // �÷��̾ �̹� �����ϸ� Ȱ��ȭ
                    players[playerIndices[body.TrackingId]].SetActive(true);
                }
                UpdatePlayerPosition(body); // �÷��̾� ��ġ ������Ʈ
            }
        }
    }

    private void RemoveLostPlayers()
    {
        lostIds.Clear(); // �������� �ʴ� ID ����Ʈ�� �ʱ�ȭ

        foreach (ulong id in playerIndices.Keys)
        {
            if (!trackedIds.Contains(id))
            {
                lostIds.Add(id); // �������� �ʴ� ID ����Ʈ�� �߰�
            }
        }

        foreach (ulong id in lostIds)
        {
            // �������� �ʴ� �÷��̾� ������Ʈ ��Ȱ��ȭ
            players[playerIndices[id]].SetActive(false);
        }
    }

    private void UpdatePlayerPosition(Body body)
    {
        var spineBasePosition = body.Joints[JointType.SpineBase].Position;
        var unitySpineBasePosition = new Vector3(spineBasePosition.X, spineBasePosition.Y, -spineBasePosition.Z);

        // Kinect �����͸� ������� ���ο� ��ġ ���
        int playerIndex = playerIndices[body.TrackingId];
        GameObject player = players[playerIndex];
        Vector3 initialPosition = player.transform.position; // �÷��̾��� �ʱ� ��ġ ��������

        // ��ǥ�� �ʱ� ��ġ���� �¿�θ� �̵��ϵ��� ������Ʈ
        var newPosition = new Vector3(unitySpineBasePosition.x * moveSpeed, initialPosition.y, initialPosition.z);

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
