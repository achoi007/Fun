import TwentyFortyEight

class TwentyFortyEightPlayer:

    def __init__(self):
        pass

    def run(self):
        self.game = TwentyFortyEight(4, 4)
        game = self.game
        # Read command and process it
        cmd = input("(u)p, (l)eft, (r)ight, (d)own, (R)eset, (S)ize")
        if cmd == "u":
            game.move(TwentyFortyEight.UP)
        elif cmd == "l":
            game.move(TwentyFortyEight.UP)
        elif cmd == "r":
            game.move(TwentyFortyEight.UP)
        elif cmd == "d":
            game.move(TwentyFortyEight.UP)
        elif cmd == "R":
            print("Game is reset")
            game.reset()
        elif cmd == "S":
            width = input("New width")
            height = input("New height")
            game = TwentyFortyEight(height, width)
        else:
            print("Unknown command: ", cmd)
        # Print out current game
        if game.is_won():
            print("Congrats, you have won the game")
        elif game.is_lost():
            print("Sorry, you have lost.")
            game.reset()
        print(game)

if __name__ == '__main__':
    player = TwnetyFortyEightPlayer()
    player.run()
    
        
