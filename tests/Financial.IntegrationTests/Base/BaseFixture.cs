using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.IntegrationTests.Base;
public class BaseFixture
{
    public BaseFixture()
    => Faker = new Faker("pt_BR");
    

    protected Faker Faker {  get; set; }

}
