using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalSeeker : MonoBehaviour
{
    Goal[] mGoals;
    Action[] mActions;
    Action mChangeOverTime;
    const float TICK_LENGTH = 3.0f;

    public Text discontentmentText;
    public Text currentActionText;
    public Text testText;
    public Text timeText;

    public Vector3 kitchenPos;
    public Vector3 bedroomPos;
    public Vector3 couchPos;
    public Vector3 restroomPos;
    public Vector3 televisionPos;

    public GameObject soda;
    public GameObject snack;
    public GameObject raw;
    public GameObject tvScreen;

    public AudioSource drinkAudio;
    public AudioSource eatAudio;
    private void Start()
    {
        mGoals = new Goal[4];
        mGoals[0] = new Goal("Eat", 4);
        mGoals[1] = new Goal("Sleep", 3);
        mGoals[2] = new Goal("Bathroom", 3);
        mGoals[3] = new Goal("Entertainment", 2);

        //The actions I know how to do
        mActions = new Action[7];
        mActions[0] = new Action("eat some raw food");
        mActions[0].targetGoals.Add(new Goal("Eat", -4f));
        mActions[0].targetGoals.Add(new Goal("Sleep", +1f));
        mActions[0].targetGoals.Add(new Goal("Bathroom", +1f));
        mActions[0].targetGoals.Add(new Goal("Entertainment", +1f));

        mActions[1] = new Action("eat a snack");
        mActions[1].targetGoals.Add(new Goal("Eat", -2f));
        mActions[1].targetGoals.Add(new Goal("Sleep", +1f));
        mActions[1].targetGoals.Add(new Goal("Bathroom", +1f));
        mActions[1].targetGoals.Add(new Goal("Entertainment", 0f));

        mActions[2] = new Action("sleep in a bed");
        mActions[2].targetGoals.Add(new Goal("Eat", +2f));
        mActions[2].targetGoals.Add(new Goal("Sleep", -4f));
        mActions[2].targetGoals.Add(new Goal("Bathroom", +2f));
        mActions[2].targetGoals.Add(new Goal("Entertainment", 0f));

        mActions[3] = new Action("sleep on the couch");
        mActions[3].targetGoals.Add(new Goal("Eat", +1f));
        mActions[3].targetGoals.Add(new Goal("Sleep", -2f));
        mActions[3].targetGoals.Add(new Goal("Bathroom", +1f));
        mActions[3].targetGoals.Add(new Goal("Entertainment", +1f));

        mActions[4] = new Action("drink a soda");
        mActions[4].targetGoals.Add(new Goal("Eat", -2f));
        mActions[4].targetGoals.Add(new Goal("Sleep", -3f));
        mActions[4].targetGoals.Add(new Goal("Bathroom", +2f));
        mActions[4].targetGoals.Add(new Goal("Entertainment", +2f));

        mActions[5] = new Action("use the restroom");
        mActions[5].targetGoals.Add(new Goal("Eat", 0f));
        mActions[5].targetGoals.Add(new Goal("Sleep", 0f));
        mActions[5].targetGoals.Add(new Goal("Bathroom", -4f));
        mActions[5].targetGoals.Add(new Goal("Entertainment", +1f));

        mActions[6] = new Action("watch TV");
        mActions[6].targetGoals.Add(new Goal("Eat", +2f));
        mActions[6].targetGoals.Add(new Goal("Sleep", +1f));
        mActions[6].targetGoals.Add(new Goal("Bathroom", +2f));
        mActions[6].targetGoals.Add(new Goal("Entertainment", -3f));

        //the rate of the goals change as a result of time passing
        mChangeOverTime = new Action("tick");
        mChangeOverTime.targetGoals.Add(new Goal("Eat", +4f));
        mChangeOverTime.targetGoals.Add(new Goal("Sleep", +3f));
        mChangeOverTime.targetGoals.Add(new Goal("Bathroom", +2f));
        mChangeOverTime.targetGoals.Add(new Goal("Entertainment", +3f));

        Debug.Log("Starting clock. One hour will pass every " + TICK_LENGTH + " seconds.");
        InvokeRepeating("Tick", 0f, TICK_LENGTH);

        Debug.Log("Press E to do something");
    }

    void Tick()
    {
        //apply change over time
        foreach (Goal goal in mGoals)
        {
            goal.value += mChangeOverTime.GetGoalChange(goal);
            goal.value = Mathf.Max(goal.value, 0f);
        }

        PrintGoals();
    }

    void PrintGoals()
    {
        string goalString = "";
        foreach (Goal goal in mGoals)
        {
            goalString += goal.name + ": " + goal.value + "; ";
        }
        goalString += "Discontenment: " + CurrentDiscontentment();
        Debug.Log(goalString);
        discontentmentText.text = goalString.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pressed E");
            Action bestThingToDo = ChooseAction(mActions, mGoals);
            Debug.Log("I think I will " + bestThingToDo.name);

            //do the thing
            foreach (Goal goal in mGoals)
            {
                goal.value += bestThingToDo.GetGoalChange(goal);
                goal.value = Mathf .Max(goal.value, 0f);
            }

            if (bestThingToDo == mActions[0])
            {
                transform.position = kitchenPos;
                eatAudio.Play();
                raw.SetActive(true);
                StartCoroutine(fruitCountdown());
                Debug.Log("I am currently eating raw food");
                testText.text = bestThingToDo.name.ToString();
            }

            if (bestThingToDo == mActions[1])
            {
                transform.position = kitchenPos;
                eatAudio.Play();
                snack.SetActive(true);
                StartCoroutine(fryCountdown());
                Debug.Log("I am currently eating a snack food");
                testText.text = bestThingToDo.name.ToString();
            }

            if (bestThingToDo == mActions[2])
            {
                transform.position = bedroomPos;
                Debug.Log("I am currently sleeping on the bed");
                testText.text = bestThingToDo.name.ToString();
            }

            if (bestThingToDo == mActions[3])
            {
                transform.position = couchPos;
                Debug.Log("I am currently sleeping on the couch");
                testText.text = bestThingToDo.name.ToString();
            }

            if (bestThingToDo == mActions[4])
            {
                transform.position = kitchenPos;
                drinkAudio.Play();
                soda.SetActive(true);
                StartCoroutine(sodaCountdown());
                Debug.Log("I am currently drinking a soda");
                testText.text = bestThingToDo.name.ToString();
            }

            if (bestThingToDo == mActions[5])
            {
                transform.position = restroomPos;
                Debug.Log("I am currently using the restroom");
                testText.text = bestThingToDo.name.ToString();
            }

            if (bestThingToDo == mActions[6])
            {
                transform.position = televisionPos;
                tvScreen.SetActive(true);
                StartCoroutine(tvCountdown());
                Debug.Log("I am currently watching television");
                testText.text = bestThingToDo.name.ToString();
            }

            PrintGoals();
        }
    }

    Action ChooseAction(Action[] actions, Goal[] goals)
    {
        //find the best action leading to the lowest discontentment
        Action bestAction = null;
        float bestValue = float.PositiveInfinity;

        foreach (Action action in actions)
        {
            float thisValue = Discontentment(action, goals);

            if (thisValue < bestValue)
            {
                bestValue = thisValue;
                bestAction = action;
            }
        }

        return bestAction;
    }

    float Discontentment(Action action, Goal[] goals)
    {
        //keep a running total
        float discontentment = 0f;

        //loop through each goal
        foreach(Goal goal in goals)
        {
            //calculate the new value after the action
            float newValue = goal.value + action.GetGoalChange(goal);
            newValue = Mathf.Max(newValue, 0);

            //get the discontentment of this value
            discontentment += goal.GetDiscontentment(newValue);
        }

        return discontentment;
    }

    float CurrentDiscontentment()
    {
        float total = 0f;
        foreach (Goal goal in mGoals)
        {
            total += (goal.value * goal.value);
        }

        return total;
    }

    IEnumerator fryCountdown()
    {
        yield return new WaitForSeconds(1f);
        snack.SetActive(false);
    }

    IEnumerator fruitCountdown()
    {
        yield return new WaitForSeconds(1f);
        raw.SetActive(false);
    }

    IEnumerator sodaCountdown()
    {
        yield return new WaitForSeconds(1f);
        soda.SetActive(false);
    }

    IEnumerator tvCountdown()
    {
        yield return new WaitForSeconds(1f);
        tvScreen.SetActive(false);
    }
}
