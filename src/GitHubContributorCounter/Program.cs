using Octokit;

var uriString = args[0];
var pat = args.Length > 1 ? args[1] : null;
if (string.IsNullOrEmpty(uriString) ||
    !Uri.TryCreate(uriString, UriKind.Absolute, out var uri))
{
    Console.WriteLine("Please enter a GitHub Project or Organization URL");
    return;
}

if (string.IsNullOrEmpty(pat))
{
    Console.WriteLine("Note that you should use a PAT (Personal Access Token) to avoid rate-limiting.");
}


var options = new ApiOptions()
{
    PageSize = 100
};

var orgOrUserName = uri.Segments[1].Trim('/');
var repositoryName = uri.Segments.Length > 2 ? uri.Segments[2].Trim('/') : null;

var client = new GitHubClient(new ProductHeaderValue("GitHubContributorCounter"));
if (!string.IsNullOrEmpty(pat))
{
    client.Credentials = new Credentials(pat);
}

bool isOrganization = false;
try
{
    await client.Organization.Get(orgOrUserName);
    isOrganization = true;
}
catch (NotFoundException)
{
    isOrganization = false;
}

if (repositoryName is not null)
{
    var contributors = await client!.Repository.GetAllContributors(orgOrUserName, repositoryName, options);
    Console.WriteLine($"Number of contributors is {contributors.Count}");
    return;
}

var repositories = isOrganization ? 
    await client.Repository.GetAllForOrg(orgOrUserName, options) : 
    await client.Repository.GetAllForUser(orgOrUserName, options);

var uniqueContributors = new HashSet<string>();

foreach (var repository in repositories)
{
    if (repository.Fork)
    {
        continue;
    }

    var contributors = await client!.Repository.GetAllContributors(orgOrUserName, repository.Name, options);
    foreach (var contributor in contributors)
    {
        uniqueContributors.Add(contributor.Login);
    }
}

Console.WriteLine($"Number of contributors is {uniqueContributors.Count}");