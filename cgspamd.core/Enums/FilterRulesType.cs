using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.core.Enums
{
    enum FilterRulesType
    {
        whiteListSenderDomains,
        whiteListSenderAddresses,
        blackListSenderDomains,
        blackListSenderAddresses,
        excludedRecipients,
        prohibitedTextInBody,
        prohibitedRegExInBody
    }
}
