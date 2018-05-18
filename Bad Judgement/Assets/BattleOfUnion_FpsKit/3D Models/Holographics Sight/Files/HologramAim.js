function Update () {
if(CharacterMotor.isAim){
	GetComponent(MeshRenderer).enabled=true;
}else{
		GetComponent(MeshRenderer).enabled=false;
	
}
}