#pragma strict
//http://answers.unity3d.com/questions/50801/how-do-i-make-credits.html

var speed = 0.05;
var crawling = true;
var chewy : Font;
 
function Start()
{
    // init text here, more space to work than in the Inspector (but you could do that instead)
    var tc = GetComponent(GUIText);
    //tc.transform.translate(Screen.width/2, 0);
    var creds = "Bar Crawl: A Night to Forget\n\n\n\n";
    creds += "Lead Designer\nCody Childers\n\n\n";
    creds += "Lead Developer\nSaul Winer\n\n\n";
    creds += "Lead Producer\nIan MacLeish\n\n\n";
    creds += "Lead Tester\nKyle Sullivan\n\n\n";
    creds += "Lead User Interface Designer\nKrishna Velury\n\n\n";
    creds += "Lead Artist Coordinator\nand Level Designer\nMark McGowan\n\n\n";
    creds += "Lead Level Programmer\nSteven Hack\n\n\n";
    creds += "Lead Artist\nRebecca Alto\n\n\n";
    creds += "Other Visual Artists\nAlexis Williams\nPhoebe Rothfeld\nJes Udelle\nJon Le\n\n\n";
    creds += "Lead Audio Designer\nChristopher Miller\n\n\n";
    creds += "Additional Music\nLiquid Courage by Vio-Lence\n\n\n";
    creds += "Voice acting\nLucahjin\n\n\n";
    creds += "Special Thanks\nDavid Wessman\nVio-Lence\n\n\n";
    tc.text= creds; 
    tc.color = Color.white;
    tc.font = chewy;
    //tc.fontSize = 20;
    tc.fontSize = Screen.width/20;//hopefully this will make it scale with screens correctly
    
}

function Update ()
{
    if (!crawling)
        return;
    transform.Translate(Vector3.up * Time.deltaTime * speed);
    if (gameObject.transform.position.y > 3.2)
    {
        Application.LoadLevel("Options");
    }
}