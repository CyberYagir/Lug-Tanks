using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon2 : Weapon
{
    public GameObject line;


    private void Start()
    {
        shootAction += () =>
        {
            var targets = Enemies(shootPoint);
            if (targets.Count == 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(shootPoint.transform.position, shootPoint.forward, out hit))
                {
                    CreateLine(shootPoint.transform.position, hit.point);
                }
                else
                {
                    CreateLine(shootPoint.transform.position, shootPoint.forward * 1000f);
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(shootPoint.transform.position, targets[0].enemy.gameObject.transform.position - shootPoint.transform.position, out hit))
                {
                    CreateLine(shootPoint.transform.position, targets[0].enemy.transform.position);
                }
            }
        };
    }

    public void CreateLine(Vector3 pos1, Vector3 pos2)
    {
        var l = Instantiate(line.gameObject);
        l.GetComponent<LineRenderer>().SetPosition(0, pos1);//l.GetComponent<LineRenderer>().SetPosition(1, new Vector3((pos2.x - pos1.x) / 2f, pos2.y - pos1.y, (pos2.y - pos1.y) / 2f));
        l.GetComponent<LineRenderer>().SetPosition(1, pos2);
    }
}
