namespace ordination_test;

using Microsoft.EntityFrameworkCore;

using Service;
using Data;
using shared.Model;

[TestClass]
public class ServiceTest
{
    private DataService service;

    [TestInitialize]
    public void SetupBeforeEachTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName: "test-database");
        var context = new OrdinationContext(optionsBuilder.Options);
        service = new DataService(context);
        service.SeedData();
    }

    [TestMethod]
    public void PatientsExist()
    {
        Assert.IsNotNull(service.GetPatienter());
    }

    [TestMethod]
    public void OpretDagligFast()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(1, service.GetDagligFaste().Count());

        service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligFaste().Count());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestAtKodenSmiderEnException()
    {
        // Arrange
        Console.WriteLine("Forbereder testen...");

        // Act
        try
        {
            // Kalder metoden med en null-værdi for patientId for at forvente en ArgumentNullException.
            service.OpretDagligFast(null, 1, 2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

            // Assert
            // Forventer, at en ArgumentNullException bliver kastet.
            // ExpectedException-attributten vil håndtere selve påstanden for dig.
            Console.WriteLine("Her kommer der en exception. Testen lykkes ikke, hvis denne linje nås.");
        }
        catch (ArgumentNullException ex)
        {
            // Her kan du tilføje yderligere assert-statements, hvis nødvendigt.
            // For eksempel kan du verificere, at den korrekte parameter udløste undtagelsen.
            Assert.AreEqual("patientId", ex.ParamName, "Forventet ArgumentNullException for patientId");

            // Alternativt kan du blot lade dette område være tomt, da du allerede forventer en exception.
        }
    }


}