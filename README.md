# School-kwaliteit

## Korte uitleg repo

Dit is voor het vak dat gaat over kwaliteit ISO 25010 enzo.

Omdat de code geschreven voor dit project eigenlijk voor persoonlijk gebruik bedoeld was zal ik hier een copy plaatsen met andere namen.

## CONTEXT

De unit tests zijn geschreven voor een Command Line tool. Het idee is dat ik makkelijker zelf CLI programmas kan schrijven.
Zijn er betere open source oplossingen? Maar natuurlijk. Echter wou ik er zelf een schrijven zodat ik er 100% controle over heb! (en omdat het leuk is om te schrijven!)



## API test code

Voor testen UI en API is er Cypress gebruikt. De integratie test is zichtbaar als Github action. De test word uitgevoerd voor elke pull request (ik heb hem als voorbeeld handmatig uitgevoerd).

De test kan ook uitgevoerd worden met het command:
`npx cypress run`


Hier is een voorbeeld van de mogelijke console output gegeven
```bash
DevTools listening on ...

====================================================================================================

  (Run Starting)

  ┌────────────────────────────────────────────────────────────────────────────────────────────────┐
  │ Cypress:        12.14.0                                                                        │
  │ Browser:        Electron 106 (headless)                                                        │
  │ Node Version:   v18.15.0 (C:\Program Files\nodejs\node.exe)                                    │
  │ Specs:          1 found (spec.cy.js)                                                           │
  │ Searched:       cypress/e2e/**/*.cy.{js,jsx,ts,tsx}                                            │
  └────────────────────────────────────────────────────────────────────────────────────────────────┘


────────────────────────────────────────────────────────────────────────────────────────────────────

  Running:  spec.cy.js                                                                      (1 of 1)


  Cith pages
    √ Check CV (1382ms)
    √ Check Games Page (164ms)
    √ Check Privacy Page (153ms)
    √ Check Bad Login (15903ms)

  Cith API School
    √ Get list (48ms)
    √ Get school (34ms)
    √ POST school/5 (32ms)
    √ POST school/5 error 409 on second post (38ms)
    √ PUT school/6 and GET same value (65ms)
    √ DELETE school (48ms)


  10 passing (19s)


  (Results)

  ┌────────────────────────────────────────────────────────────────────────────────────────────────┐
  │ Tests:        10                                                                               │
  │ Passing:      10                                                                               │
  │ Failing:      0                                                                                │
  │ Pending:      0                                                                                │
  │ Skipped:      0                                                                                │
  │ Screenshots:  0                                                                                │
  │ Video:        true                                                                             │
  │ Duration:     19 seconds                                                                       │
  │ Spec Ran:     spec.cy.js                                                                       │
  └────────────────────────────────────────────────────────────────────────────────────────────────┘


  (Video)

  -  Started compressing: Compressing to 32 CRF
  -  Finished compressing: 1 second

  -  Video output: ...


====================================================================================================

  (Run Finished)


       Spec                                              Tests  Passing  Failing  Pending  Skipped
  ┌────────────────────────────────────────────────────────────────────────────────────────────────┐
  │ √  spec.cy.js                               00:19       10       10        -        -        - │
  └────────────────────────────────────────────────────────────────────────────────────────────────┘
    √  All specs passed!                        00:19       10       10        -        -        -

```

Hier is de code van de controller die op de server staat. Ik weet dat dit eigenlijk niet 100% goed is volgens de rest regels. Maar goed genoeg voor deze test!

Dit is niet 100% de echte code, een paar dingen zijn er voor de veiligheid eruit gehaald + comments
```csharp

namespace Website.Controllers;

[Route("api/[controller]")]
public class SchoolController : Controller
{
    private readonly IEnumerable<EndpointDataSource> _endpointSources;

    private static Dictionary<int, string> data = new Dictionary<int, string>();

    public SchoolController(IEnumerable<EndpointDataSource> endpointSources) 
    {
        _endpointSources = endpointSources;
    }

    [HttpGet("List")]
    public IActionResult GetList()
    {
        var endpoints = _endpointSources
            .SelectMany(es => es.Endpoints)
            .OfType<RouteEndpoint>();
        var output = endpoints.Select(
            e =>
            {
                var controller = e.Metadata
                    .OfType<ControllerActionDescriptor>()
                    .FirstOrDefault();
                var action = controller != null
                    ? $"{controller.ControllerName}.{controller.ActionName}"
                    : null;
                var controllerMethod = controller != null
                    ? $"{controller.ControllerTypeInfo.FullName}:{controller.MethodInfo.Name}"
                    : null;
                return new
                {
                    Method = e.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods?[0],
                    Route = $"/{e.RoutePattern.RawText.TrimStart('/')}",
                    Action = action
                    //ControllerMethod = controllerMethod
                };
            }
        ).Where(x => x.Action?.StartsWith("School") ?? false);

        return Json(output);
    }


    [HttpGet]
    public ActionResult Details()
    {
        return Json(data);
    }


    [HttpGet("{id}")]
    public ActionResult OneDetail(int id)
    {
        if (!data.ContainsKey(id))
        {
            return Json("");
        }
        return Json(data[id]);
    }


    [HttpPost("Create/{id}")]
    public IResult Create(int id)
    {
        try
        {
            data.Add(id, "");
            return Results.Ok();
        }
        catch (Exception)
        {
            return Results.Conflict();
        }
    }


    [HttpPut("{id}/{newValue}")]
    public IActionResult Edit(int id, string newValue)
    {
        data[id] = newValue;
        return Json(data[id]);
    }


    [HttpDelete("{id}")]
    public IResult Delete(int id)
    {
        bool result = data.Remove(id);
        if (result)
        {
            return Results.Ok();
        }
        
        return Results.NotFound();
    }


    [HttpGet("Reset")]
    public IResult Reset()
    {
        data.Clear();
        return Results.Ok();
    }
}
```