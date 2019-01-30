using EosSharp.UnitTests.Unity3D;
using Newtonsoft.Json;
using System;
using UnityEngine;

public class UnitTestsScript : MonoBehaviour
{
    public async void UnitTestAll()
    {
        try
        {
            ApiUnitTests autc = new ApiUnitTests();
            await autc.TestAll();

            EosUnitTests eut = new EosUnitTests();
            await eut.TestAll();

            SerializationUnitTests sut = new SerializationUnitTests();
            sut.TestAll();

            SignUnitTests signUnitTests = new SignUnitTests();
            await signUnitTests.TestAll();

            StressUnitTests stressUnitTests = new StressUnitTests();
            await stressUnitTests.TestAll();
        }
        catch (Exception ex)
        {
            print(JsonConvert.SerializeObject(ex));
        }
    }
}
