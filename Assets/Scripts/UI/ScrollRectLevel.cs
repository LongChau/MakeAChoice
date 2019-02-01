using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollRectLevel : MonoBehaviour {
	
	public RectTransform panelContent;	
	public RectTransform panelItem;
	public RectTransform center;	

	private RectTransform [] panelLevels =new RectTransform[20];
	private float[] distance;
	private float[] distReposition;

	private bool dragging = false;	
	private int minLevelNum;	
	private int panelLength;

	private float disDeafult = 300f;
	private Vector2 blockLeft ;
	private Vector2 blockRigth ;

	private bool isBlock = false;

	void Start()
	{
		panelLength = panelLevels.Length;
		distance = new float[panelLength];
		distReposition = new float[panelLength];

		InitTextContent ();
		blockRigth =  panelLevels [1].position;

		////Block scroll Top
		float sizePanleX  = Mathf.Abs (panelLevels [panelLength - 1].position.x - panelLevels [0].position.x);
		float dis  = Mathf.Abs (panelLevels [1].position.x - panelLevels [0].position.x);
		float newX  =  sizePanleX - dis/2 ;
		blockLeft = new Vector2 (-newX,panelContent.position.y );

		Debug.Log (panelContent.position +" " + blockLeft +" " + sizePanleX  + " "  + newX);
	}

	void Update()
	{
		
		FindDistanceMin ();

		if (!dragging)
		{
			if(isBlock)
				BlockOpotionPanel ();
			
			if(GetComponent<ScrollRect>().velocity.x <= 15f)
				LerpToBttn (-panelLevels[minLevelNum].anchoredPosition.x);
		}
	}


	void InitTextContent ()
	{
		panelLevels [0] = panelItem;
		RectTransform rec = panelItem;

		for (int i = 1; i < 20; i++) {

			Vector2 pos = new Vector2 (i * 160f, rec.anchoredPosition.y);

			RectTransform item = Instantiate (panelItem) as RectTransform;
			item.SetParent (panelContent);
		
			item.anchoredPosition = pos;
			item.position = new Vector3 ( item.position.x,rec.position.y, rec.position.z);

			item.localScale = rec.localScale;
			panelLevels [i] = item;

		}
	}

	void FindDistanceMin()
	{
		for (int i = 0; i < panelLevels.Length; i++)
		{
			distReposition[i] = center.GetComponent<RectTransform>().position.x - panelLevels[i].position.x;
			distance[i] = Mathf.Abs(distReposition[i]);

		}

		float minDistance = Mathf.Min(distance);	// Get the min distance

		for (int a = 0; a < panelLevels.Length; a++)
		{
			if (minDistance == distance[a])
			{
				minLevelNum = a;
			}

			var dis = disDeafult - distance [a] ;

			if(dis >= 0)
			{

				panelLevels [a].localScale = Vector3.Lerp (panelLevels [a].localScale,
					Vector3.one*( 0.5f +  (dis * 0.5f)/disDeafult), Time.deltaTime * 5f	);
				

			}else
			{

				panelLevels [a].localScale = Vector3.Lerp (panelLevels [a].localScale,
					new Vector3( 0.5f   , 0.5f  , panelLevels [a].localScale.z ), Time.deltaTime * 1f	);


			}
		}
	}

	void LerpToBttn(float position)
	{
		float newX   = Mathf.Lerp (panelContent.anchoredPosition.x,position, 2.5f * Time.deltaTime);

		if(Mathf.Abs(position - newX) < 3f)
		{
			newX = position;
		}

		if((Mathf.Abs(position) >= Mathf.Abs(newX) -1) && (Mathf.Abs(position) <= Mathf.Abs(newX) + 1 )   ){

		
		}
		Vector2  newPosition = new Vector2 (newX, panelContent.anchoredPosition.y );
		panelContent.anchoredPosition = newPosition;

	}

	private void BlockOpotionPanel ()
	{
		if (panelContent.position.x >= blockRigth.x) {
			
			GetComponent<ScrollRect>().velocity = Vector2.zero;
			panelContent.position = new Vector3 (blockRigth.x, blockRigth.y, panelContent.position.z);
			isBlock = false;
		}


		if (panelContent.position.x <= blockLeft.x) {

			GetComponent<ScrollRect>().velocity = Vector2.zero;
			panelContent.position = new Vector3 (blockLeft.x, blockLeft.y, panelContent.position.z);
			isBlock = false;
		}

	}

	public void StartDrag()
	{
		dragging = true;
	}
	public void OnDrag()
	{
		BlockOpotionPanel ();
	}

	public void EndDrag()
	{
		isBlock = true;
		dragging = false;
	}
}
