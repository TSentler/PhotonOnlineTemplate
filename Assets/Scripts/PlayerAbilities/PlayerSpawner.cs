using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace PlayerAbilities
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Transform[] _spawnPoints;

        private void Start()
        {
            Spawn();
        }

        private void Spawn()
        {
            int spawnId = Random.Range(0, _spawnPoints.Length - 1);
            PhotonNetwork.Instantiate(_playerPrefab.name,
                _spawnPoints[spawnId].position, Quaternion.identity, 0);
        }
    }
}
