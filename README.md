### Web Api 

#### 01. Setup & In-Memory Repository

- .NET Web Api
- .NET CLI
- Skapa vår första entity
- Repository Pattern
- Dependency Injection

###### Skapa ett nytt Web Api-projekt

```
dotnet new webapi -n VinylCollection
```

Tryck på f5. Vid problem:

```
dotnet dev-certs https --trust
```

**task.json**

```
"group": {
	"kind": "build",
	"isDefault": true
}
```

Istället för f5:

```
dotnet watch run
```

#### 1. Skapa en Vinyl Entity

**Entities/Vinyl.cs**

```
public class Vinyl
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Artist { get; init; }
    }
```



#### 2. Repository

Vi ska nu skapa vårt repository, en klass som ansvarar för att lagra våra entities.

**Repositories/IVinylsRepository.cs**

```
public interface IVinylsRepository
    {
        Vinyl GetVinyl(Guid id);
        
        IEnumerable<Vinyl> GetVinyls();

        void CreateVinyl(Vinyl vinyl);

        void UpdateVinyl(Vinyl vinyl);

        void DeleteVinyl(Guid id);
    }
```

Det pattern vi använder oss av här kallas för **Repository Pattern**.

Vi kommer att ha en grupp klasser som fokuserar på att inkapsla logiken som krävs för att interagera med vår data.

*Mediates between the domain and data mapping layers using a collection-like interface for accessing domain objects.* 

Detta ger följande fördelar:

- Centraliserar från var vi hämtar vår data
- Code maintainability
- Decouples vårt domain model layer från vårar infrastrukturslager(loose coupling) 
- DRY

**Client** skickar en request som går till vår **Controller**.

```
POST example.com/vinyls
JSON {"artist": "Mr Artist", "title": "Album Title"}
```

Controller har tillgång till ett **Repositorty**. Det ända controllern vet är att repositoryn har en metod som heter **CreateVinyl** och vilken typ som kan skickas med.

```
_repository.CreateVinyl(vinyl);
```

Repositoryn tar hand om resten, och nu när vi endast använder oss av **In-memory data** så har vi satt vår data till en List och kan därför köra:

```
 public void CreateVinyl(Vinyl vinyl)
        {
            _collection.Add(vinyl);
        }
```

#### 3. Controller

Controllern är alltså klassen som får requesten från client.

```
namespace VinylCollection.Controllers
{
    [ApiController]
    [Route("vinyls")]
    
    public class VinylsController : ControllerBase
    {
        private readonly IVinylsRepository _repository;

        public VinylsController(IVinylsRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IEnumerable<Vinyl> GetVinyls()
        {
            var vinyls = _repository.GetVinyls();
            return vinyls;
        }
}
```

I detta exempel ser vi några intressant saker:

```
[ApiController]
```

Här säger vi att **VinylsController** kommer att vara en **ApiController** och vi får tillgång till en mängd olika saker som är relaterade till att vara en Api Controller, bland annat

```
[Route("vinyls")]
```

Som säger att i denna klass lyssnar vi efter request till url /vinyls. Vi skulle även kunna skriva

```
[Route("api/[controller]")]
```

vilken då ansvarar för requests till /apy/vinyls då man strippar av "controller" i VinylsController.

Om vi ska sätta ord på vad vi gör här

```
private readonly IVinylsRepository _repository;

public VinylsController(IVinylsRepository repository)
{
 _repository = repository;
}
```

så kan vi göra det med två Design Principer som säger:

**Program to an interface, not an implementation**

och 

**Identify the aspects of your application that vary and separate them from what stays the same**.

Vår controller class kommer att förbli densamma, men repositoryt kan komma att förändras. Vi kanske skapar en persistent databas i framtiden, vem vet...

Genom att skapa en interface för det repository vi kommer att använda så försäkrar vi oss om att sålänge repository implementerar interfacet så kommer applikationen fortsatt att fungera.

**IMemRepository**

```
public interface IVinylsRepository
    {
        Vinyl GetVinyl(Guid id);
        IEnumerable<Vinyl> GetVinyls();
        void CreateVinyl(Vinyl vinyl);
        void UpdateVinyl(Vinyl vinyl);
        void DeleteVinyl(Guid id);
    }
```

Förbättringar här skulle kunna vara att använda oss av **Generics**, men låt oss hålla det simpelt för nu.

#### 4. Dependency Injection och Dtos

**Vad är dependecy injection?**

Här har vi en klass som vi använda sig av en annan klass.

```
VinylsController -> IVinylsRepository
```

```
public VinylsController() 
{
	repository = new IVinylsRepositor();	
}
```

**Dependency Inversion Principle**

**Depend upon abstractions. Do not depend upon concrete classes.**

Denna princip handlar om att reducera dependencies, istället för att använda oss av en konkret klass, så säger vi att repot ska vara av en viss typ. 

```
Class depends on Interface
Dependencies implements Interface
```

```
public VinylsController(IRepository repository)
{
	this.repository = repository;
}
```

Vi måste registera våra dependencies i en Service Container **IServiceProvider**.

**Startup.cs**

```
  public void ConfigureServices(IServiceCollection services)
        {
            // services.AddSingleton<IVinylsRepository, VinylsRepository>();
  			...
            });
        }
```



Länkar:

https://www.youtube.com/watch?v=Z8urV5AullQ&ab_channel=CodingTutorials

https://martinfowler.com/eaaCatalog/repository.html

https://www.youtube.com/watch?v=ZXdFisA_hOY&t=4827s&ab_channel=freeCodeCamp.org

https://www.youtube.com/watch?v=x6C20zhZHw8&ab_channel=CodingConcepts











