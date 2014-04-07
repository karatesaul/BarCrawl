using UnityEngine;
using System.Collections;

public interface IEnemy {

	GameObject player { get; set; }
	bool isExecuting { get; set; }

}
