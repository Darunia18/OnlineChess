# OnlineChess
Online two player chess game written in Visual C#

This game was created for my Networking Concepts and Administration final project. Core files for this project were modified from an earlier "Solitaire Chess" project of mine for my Computer Science II course. For this project, I modified the game to add turn-taking, adapt the rules to fit a standard game of chess, and added network functionalities.

Feel free to further modify this game. Just be sure to update the version number in MainMenuForm.cs if you modify any network code.

## How it works
One player hosts the game while the other connects as a client through the host's IP address and chosen port. Before the game starts, the client and host confirm that they are both running on the same version of the game. If they are, the game starts with the client controlling the white pieces, therefore going first, and the host controlling the black pieces.

Players take turns moving pieces until one of the king pieces is captured. There are three ways the game can end: by winning/losing the game, by one player disconnecting, or by one player being dcaught cheating through an illegal move. If any of these conditions occur, the connection is cut, the player is notified of the outcome, and the game board is kept open until the player closes it.
