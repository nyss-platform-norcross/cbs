/*---------------------------------------------------------------------------------------------
*  Copyright (c) The International Federation of Red Cross and Red Crescent Societies. All rights reserved.
*  Licensed under the MIT License. See LICENSE in the project root for license information.
*--------------------------------------------------------------------------------------------*/

using Concepts;
using Dolittle.ReadModels;

namespace Read.DataCollectors
{
    public class Region : IReadModel
    {
        public RegionName Id { get; set; }
        public RegionName Name { get; set; }
    }
}
