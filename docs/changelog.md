## v3.0.0
### Breaking changes
v3 removes external dependencies from the core TestStack.ConventionTest project. 

1. Mono.Cecil is now ILMerged internally  
So if you use the Mono.Cecil functionality, leave it installed. Otherwise uninstall it to remove unneeded dependencies
2. ApprovalTests is no longer a dependency  
This also means the `Convention.IsWithApprovedExeptions` method is gone.  
Replace usage with `var failures = Convention.GetFailures` then use Shouldly's `failures.ShouldMatchApproved()` extension or `ApprovalTests.Verify(failures)` to verify the failures.