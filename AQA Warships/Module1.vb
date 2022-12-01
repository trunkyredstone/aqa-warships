'Skeleton Program for the AQA AS Paper 1 Summer 2016 examination
'this code should be used in conjunction with the Preliminary Material
'written by the AQA Programmer Team
'developed in the Visual Studio 2008 programming environment

'Version Number 1.0
Imports System.IO

Module Module1
    Const TrainingGame As String = "Training.txt"

    Private Structure Ship
        Dim Name As String
        Dim Size As Integer
    End Structure

    Private Sub GetRowColumn(ByRef row As Integer, ByRef column As Integer, ByRef board(,) As Char)
        Console.WriteLine()
        Dim gotInput = False
        
        Do Until column >= 0 And column < board.GetLength(1) And gotInput
            Try
                gotInput = False
                Console.Write("Please enter column: ")
                column = Console.ReadLine()
                gotInput = True
            Catch
                Console.WriteLine("Please enter an integer")
            End Try
        Loop

        gotInput = False
        
        Do Until row >= 0 And row < board.GetLength(0) And gotInput
            Try
                gotInput = False
                Console.Write("Please enter row: ")
                row = Console.ReadLine()
                gotInput = True
            Catch
                Console.WriteLine("Please enter an integer")
            End Try
        Loop
        
        Console.WriteLine()
    End Sub

    Private Sub MakePlayerMove(ByRef board(,) As Char, ByRef ships() As Ship, ByRef misses As Integer)
        Dim row As Integer
        Dim column As Integer
        GetRowColumn(row, column, board)
        If board(row, column) = "m" Or board(row, column) = "h" Then
            Console.WriteLine(
                "Sorry, you have already shot at the square (" & column & "," & row & "). Please try again.")
        ElseIf board(row, column) = "-" Then
            Console.WriteLine("Sorry, (" & column & "," & row & ") is a miss.")
            misses += 1
            board(row, column) = "m"
        Else
            Console.WriteLine("Hit at (" & column & "," & row & ").")
            If GetShipCount(board, board(row, column)) Then
                Dim hitShip As String
                For Each ship In ships
                    If board(row, column) = ship.Name(0) Then
                        hitShip = ship.Name
                    End If
                Next
                Console.WriteLine("You sunk a {0}!", hitShip)
            End If
            board(row, column) = "h"
        End If
    End Sub

    Private Sub SetUpBoard(ByRef board(,) As Char)
        Dim row As Integer
        Dim column As Integer
        For row = 0 To 9
            For column = 0 To 9
                board(row, column) = "-"
            Next
        Next
    End Sub

    Private Sub LoadGame(filename As String, ByRef board(,) As Char)
        Dim row As Integer
        Dim column As Integer
        Dim line As String
        Using fileReader = New StreamReader(filename)
            For row = 0 To 9
                line = fileReader.ReadLine()
                For column = 0 To 9
                    board(row, column) = line(column)
                Next
            Next
        End Using
    End Sub

    Private Sub PlaceRandomShips(ByRef board(,) As Char, ships() As Ship)
        Dim valid As Boolean
        Dim row As Integer
        Dim column As Integer
        Dim orientation As Char
        Dim horV As Integer
        For Each Ship In ships
            valid = False
            While Not valid
                row = Int(Rnd()*10)
                column = Int(Rnd()*10)
                horV = Int(Rnd()*2)
                If horV = 0 Then
                    orientation = "v"
                Else
                    orientation = "h"
                End If
                valid = ValidateBoatPosition(board, Ship, row, column, orientation)
            End While
            Console.WriteLine("Computer placing the " & Ship.Name)
            PlaceShip(board, Ship, row, column, orientation)
        Next
    End Sub

    Private Sub PlaceShip(ByRef board(,) As Char, ship As Ship, row As Integer, column As Integer, orientation As Char)
        Dim scan As Integer
        If orientation = "v" Then
            For scan = 0 To ship.Size - 1
                board(row + scan, column) = ship.Name(0)
            Next
        ElseIf orientation = "h" Then
            For scan = 0 To ship.Size - 1
                board(row, column + scan) = ship.Name(0)
            Next
        End If
    End Sub

    Private Function ValidateBoatPosition(board(,) As Char, ship As Ship, row As Integer, column As Integer,
                                  orientation As Char)
        Dim scan As Integer
        If orientation = "v" And row + ship.Size > 10 Then
            Return False
        ElseIf orientation = "h" And column + ship.Size > 10 Then
            Return False
        Else
            If orientation = "v" Then
                For scan = 0 To ship.Size - 1
                    If board(row + scan, column) <> "-" Then
                        Return False
                    End If
                Next
            ElseIf (orientation = "h") Then
                For scan = 0 To ship.Size - 1
                    If board(row, column + scan) <> "-" Then
                        Return False
                    End If
                Next
            End If
        End If
        Return True
    End Function

    Private Function CheckWin(board(,) As Char)
        Dim row As Integer
        Dim column As Integer
        For row = 0 To 9
            For column = 0 To 9
                If _
                    board(row, column) = "A" Or board(row, column) = "B" Or board(row, column) = "S" Or
                    board(row, column) = "D" Or board(row, column) = "P" Then
                    Return False
                End If
            Next
        Next
        Return True
    End Function
    
    Private Function GetShipCount(board(,) As Char, ship As Char) As Integer
        Dim row As Integer
        Dim column As Integer
        Dim count As Integer
        For row = 0 To 9
            For column = 0 To 9
                If board(row, column) = ship Then
                    count += 1
                End If
            Next
        Next
        Return count
    End Function

    Private Sub PrintBoard(board(,) As Char, showShips As Boolean)
        Dim row As Integer
        Dim column As Integer
        Console.WriteLine()
        Console.WriteLine("The board looks like this: ")
        Console.WriteLine()
        Console.Write(" ")
        For column = 0 To 9
            Console.Write(" " & column & "  ")
        Next
        Console.WriteLine()
        For row = 0 To 9
            Console.Write(row & " ")
            For column = 0 To 9
                If board(row, column) = "-" Then
                    Console.Write(" ")
                ElseIf _
                    board(row, column) = "A" Or board(row, column) = "B" Or board(row, column) = "S" Or
                    board(row, column) = "D" Or board(row, column) = "P" Or board(row, column) = "T" Then
                    If showShips Then
                        Console.Write(board(row, column))
                    Else
                        Console.Write(" ")
                    End If
                Else
                    Console.Write(board(row, column))
                End If
                If column <> 9 Then
                    Console.Write(" | ")
                End If
            Next
            Console.WriteLine()
        Next
    End Sub

    Private Sub DisplayMenu()
        Console.WriteLine("MAIN MENU")
        Console.WriteLine()
        Console.WriteLine("1. Start new game")
        Console.WriteLine("2. Load training game")
        Console.WriteLine("9. Quit")
        Console.WriteLine()
    End Sub

    Private Function GetMainMenuChoice()
        Dim choice As Integer
        
        Do
            Try
                Console.Write("Please enter your choice: ")
                choice = Console.ReadLine()
                Console.WriteLine()
                Return choice
            Catch
                Console.WriteLine("Please enter an integer")
            End Try
        Loop
    End Function

    Private Sub PlayGame(board(,) As Char, ships() As Ship, showShips As Boolean)
        Dim gameDone As Boolean
        Dim misses As Integer
        Do
            PrintBoard(board, showShips)
            MakePlayerMove(board, ships, misses)
            gameDone = CheckWin(board)
            If gameDone Then
                Console.WriteLine("All ships sunk!")
                Console.WriteLine()
            ElseIf misses >= 5 Then
                Console.WriteLine("You missed 5 times! Game over!")
                Console.WriteLine()
                gameDone = True
            End If
        Loop Until gameDone
    End Sub

    Private Sub SetUpShips(ByRef ships() As Ship)
        ships(0).Name = "Aircraft Carrier"
        ships(0).Size = 5
        ships(1).Name = "Battleship"
        ships(1).Size = 4
        ships(2).Name = "Submarine"
        ships(2).Size = 3
        ships(3).Name = "Destroyer"
        ships(3).Size = 3
        ships(4).Name = "Patrol Boat"
        ships(4).Size = 2
        ships(5).Name = "Tugboat"
        ships(5).Size = 2
    End Sub

    Sub Main()
        Dim board(9, 9) As Char
        Dim ships(5) As Ship
        Dim menuOption As Integer
        Do
            SetUpBoard(board)
            SetUpShips(ships)
            DisplayMenu()
            menuOption = GetMainMenuChoice()
            If menuOption = 1 Then
                PlaceRandomShips(board, ships)
                PlayGame(board, ships, False)
            ElseIf menuOption = 2 Then
                LoadGame(TrainingGame, board)
                PlayGame(board, ships, True)
            End If
        Loop Until menuOption = 9
    End Sub
End Module