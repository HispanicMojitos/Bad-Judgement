var Blood1:AudioClip;
var Blood2:AudioClip;
var Blood3:AudioClip;
var Sayi:int;
var Timx:float=0.45;
var Just:boolean=false;
function Start () {
if(Just){
Sayi=Random.Range(1,4);
if(Sayi==1){
AudioSource.PlayClipAtPoint(Blood1,transform.position);
}
if(Sayi==2){
AudioSource.PlayClipAtPoint(Blood2,transform.position);
}
if(Sayi==3){
AudioSource.PlayClipAtPoint(Blood3,transform.position);
}
}

yield WaitForSeconds(Timx);
Destroy(gameObject);
}

