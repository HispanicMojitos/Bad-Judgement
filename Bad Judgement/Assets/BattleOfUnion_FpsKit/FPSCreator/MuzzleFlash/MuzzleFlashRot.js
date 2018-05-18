static var Fire:boolean=false;
static var FireS:boolean=false;
function Update () 
{
if(GetComponent(MeshRenderer).enabled==true){
GetComponent(MeshRenderer).enabled=false;
}
if(GetComponent(Light).enabled==true){
GetComponent(Light).enabled=false;
}
if(Fire){
GetComponent(MeshRenderer).enabled=true;
GetComponent(Light).enabled=true;
Fire=false;
}
	transform.Rotate (30, 0 * Time.deltaTime, 0);
}
