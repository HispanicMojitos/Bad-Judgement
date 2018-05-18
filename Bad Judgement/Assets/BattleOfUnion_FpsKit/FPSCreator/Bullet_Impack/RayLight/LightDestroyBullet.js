#pragma strict

function Start () {
yield WaitForSeconds(3.5);
Destroy(gameObject);
}

function OnTriggerEnter (col : Collider) {
if(col){
	// Destroy(gameObject);
	
}
}