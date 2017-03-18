// this is a unit documentation
// next line
// next line
unit UTest;

interface

  // this is a class documentation
  // next line
  // next line
  type TEmptyClass = class

  end;

	type

    // this is the next class documentation
    // next line
    // next line
		TTest = class

			private

		TestMember: Integer; //testinline comment
        // a member comment
				privateMember1:
          Boolean;
        privateMember2 : Integer;
        privateMember3 : Double ;             // inline comment

			private	procedure privateProcedure1;
			private	function privateFunction1(param1: Integer; param2: Double): Boolean;
        function privateFunction2(param1: Integer) : Integer ;
        function privateFunction3 : Double ;

			protected
				protectedMember1: Boolean;
        protectedMember2 : Integer;
        protectedMember3 : Double ;

      protected	procedure protectedProcedure1;

        // a procedure comment
				function protectedFunction1: Boolean;
        function protectedFunction2 : Integer;
        function protectedFunction3 : Double ;

			public
				publicMember1: Boolean;
        publicMember2 : Integer;
        publicMember3 : Double ;

			public procedure publicProcedure1(param1: Double; 
											param2 : TList<Integer>);
				function publicFunction1: Boolean;
        function publicFunction2 : Integer;
        function publicFunction3 : Double ;

		end;

implementation

{ TTest }

{ TTest }

function TTest.privateFunction1(param1: Integer; param2: Double): Boolean;
begin

end;

// function comment
//
function TTest.privateFunction2(param1: Integer): Integer;
begin

end;

function TTest.privateFunction3: Double;
begin

end;

procedure TTest.privateProcedure1;
begin

end;

function TTest.protectedFunction1: Boolean;
begin

end;

function TTest.protectedFunction2: Integer;
begin

end;

function TTest.protectedFunction3: Double;
begin

end;

procedure TTest.protectedProcedure1;
begin

end;

function TTest.publicFunction1: Boolean;
begin

end;

function TTest.publicFunction2: Integer;
begin

end;

function TTest.publicFunction3: Double;
begin

end;

procedure TTest.publicProcedure1;
begin

end;

end.
