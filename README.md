# GitHub Contributor Counter

Simple .NET app to count all unique contributors in a GitHub project or organization.

## Usage

To avoid rate-limiting of GitHub API, replace PAT below with your personal access token (see [here](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token) how to generate it).

### Counting contributors for a given repository

```cli
./GithubContributorCounter.exe https://github.com/YourOrgOrUser/RepositoryName PAT
```

### Counting contributors for a given organization/user

```cli
./GithubContributorCounter.exe https://github.com/YourOrgOrUser PAT
```
