
module DailyProgrammer.Challenge122Easy
open System
open NUnit.Framework

// [04/01/13] Challenge #122 [Easy] Sum Them Digits
// http://www.reddit.com/r/dailyprogrammer/comments/1berjh/040113_challenge_122_easy_sum_them_digits/

let rec DigitalRoot (number: string) =
    let res = Seq.sumBy (fun c -> int c - int '0') number
    if res < 10 then res else DigitalRoot (res.ToString())

[<TestFixture>]
type Test() = 
        [<Test>]
        member x.``Digital root of 0 is 0``  () =
            Assert.AreEqual(0, DigitalRoot "0")
        
        [<Test>]
        member x.``Digital root of 31337 is 8`` () =
            Assert.AreEqual(8, DigitalRoot "31337")
            
        [<Test>]
        member x.``Digital root of 1073741824 is 1`` () =
            Assert.AreEqual(1, DigitalRoot "1073741824")