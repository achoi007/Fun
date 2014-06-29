import random
import TwentyFortyEight as tfe

class TwentyFortyEightPlayer:

    def __init__(self):
        pass

    def run(self):
        width = int(raw_input("width: "))
        height = int(raw_input("height: "))
        self.game = tfe.TwentyFortyEight(width, height, tfe.standard_tile_func)
        game = self.game
        game.new_game()
        while not(game.is_won()) and not(game.is_lost()):
            print game
            # Read command and process it
            cmd = raw_input("(u)p, (l)eft, (r)ight, (d)own, (N)ew Game: ")
            if cmd == "u":
                game.move(tfe.TwentyFortyEight.UP)
            elif cmd == "l":
                game.move(tfe.TwentyFortyEight.LEFT)
            elif cmd == "r":
                game.move(tfe.TwentyFortyEight.RIGHT)
            elif cmd == "d":
                game.move(tfe.TwentyFortyEight.DOWN)
            elif cmd == "N":
                game.reset()
                game.new_game()
            else:
                print("Unknown command: ", cmd)
        # Print out current game
        print game
        if game.is_won():
            print("Congrats, you have won the game")
        elif game.is_lost():
            print("Sorry, you have lost.")

if __name__ == '__main__':
    player = TwentyFortyEightPlayer()
    player.run()
    
        
