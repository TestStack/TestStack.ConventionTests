namespace TestStack.ConventionTests.Internal
{
    using ApprovalTests;
    using ApprovalTests.Core;

    public class ConventionTestsApprovalTextWriter : ApprovalTextWriter, IApprovalWriter
    {
        readonly int count;


        public ConventionTestsApprovalTextWriter(string formattedResult, int count, string extensionWithoutDot)
            : base(formattedResult, extensionWithoutDot)
        {
            this.count = count;
        }

        string IApprovalWriter.GetApprovalFilename(string basename)
        {
            if (count == 0)
            {
                return GetApprovalFilename(basename);
            }
            return GetApprovalFilename(basename + count);
        }

        string IApprovalWriter.GetReceivedFilename(string basename)
        {
            if (count == 0)
            {
                return GetReceivedFilename(basename);
            }
            return GetReceivedFilename(basename + count);
        }
    }
}