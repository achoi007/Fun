import numpy as np
import math
import unittest
import random

def standard_tile_func(*args):
    p = random.randrange(0, 100)
    if p < 10:
        return 4
    else:
        return 2

class TwentyFortyEight:
    '''
    The famous 2048 game (http://gabrielecirulli.github.io/2048/) in Python using Numpy.
    '''

    UP = 1
    DOWN = 2
    LEFT = 3
    RIGHT = 4

    def __init__(self, height, width, new_tile_func):
        '''
        Initialize empty grid to dimension [height x width] 
        '''
        self._width = width
        self._height = height
        self._new_tile_func = new_tile_func
        self.reset()


    def reset(self):
        '''
        Reset all tiles to empty
        '''
        self._tiles = np.zeros((self._height, self._width), int)

    def new_game(self):
        self.reset()
        self.add_random_tile()
        self.add_random_tile()


    def get_width(self):
        '''
        Get width (number of columns)
        '''
        return self._width


    def get_height(self):
        '''
        Get height (number of rows)
        '''
        return self._height

    def add_random_tile(self):
        '''
        Set a random empty tile to random tile number and return coordinate of new tile
        '''
        tile = self._new_tile_func()
        if tile == 0:
            return None
        index = self._find_random_empty_tile()
        if index == None:
            return None
        self.set_tile(index[0], index[1], tile)
        return index


    def move(self, direction):
        '''
        Move board to given direction.  Movement causes tiles to be merged
        and new tile to come up.
        '''
        # Cache some functors
        merge = self._merge
        rev = self._reverse
        # Process each direction
        if direction == TwentyFortyEight.UP:
            cols = [merge(col) for col in self._get_cols()]
            self._set_cols(cols)
        elif direction == TwentyFortyEight.DOWN:
            cols = (merge(rev(col)) for col in self._get_cols())
            self._set_cols([rev(col) for col in cols])
        elif direction == TwentyFortyEight.LEFT:
            rows = [merge(row) for row in self._get_rows()]
            self._set_rows(rows)
        elif direction == TwentyFortyEight.RIGHT:
            rows = (merge(rev(row)) for row in self._get_rows())
            self._set_rows([rev(row) for row in rows])
        else:
            raise ValueError('Illegal direction ' + direction)
        # Add new tile if it has changed
        if self._is_changed:
            tile = self._new_tile_func()
            if tile != 0:
                tiles = self._tiles
                # Pick a random empty position
                (row, col) = self._find_random_empty_tile()
                tiles[row, col] = tile

    def _find_random_empty_tile(self):
        empties = np.where(self._tiles == 0)
        empties_len = len(empties[0])
        if empties_len == 0:
            return None
        else:
            i = random.randrange(0, empties_len)
            return (empties[0][i], empties[1][i])
            


    def get_tile(self, row, col):
        '''
        Get tile at given coordinate
        '''
        return self._tiles[row, col]


    def set_tile(self, row, col, tile):
        '''
        Set tile at given coordinate
        '''

        self._tiles[row, col] = tile

    def is_won(self):
        '''
        Return true if game is won - any tile that has 2048
        '''
        return np.any(self._tiles == 2048)


    def is_lost(self):
        '''
        Return true if game is lost - all tiles occupied and no merge is
        possible.
        '''
        # If at least 1 non empty tile, not lost
        if np.any(self._tiles == 0):
            return False
        # For each row, check tile[n] and tile[n+1] are different
        for row in self._get_rows():
            if self._has_same_elem(row):
                return False
        # For each column, check tile[n] and tile[n+1] are different
        for col in self._get_cols():
            if self._has_same_elem(col):
                return False
        # No empty, no same element between adjacent cell, game is lost
        return True

    def _has_same_elem(self, v):
        for i in range(len(v) - 1):
            if v[i] == v[i+1]:
                return True
        return False


    def _get_rows(self):
        tiles = self._tiles
        return [tiles[r, :] for r in range(self._height)]


    def _get_cols(self):
        tiles = self._tiles
        return [tiles[:, c] for c in range(self._width)]


    def _create_empty_tiles(self):
        return np.empty((self._height, self._width), int)


    def _set_rows(self, rows):
        new_tiles = self._create_empty_tiles()
        for i,row in enumerate(rows):
            new_tiles[i, :] = np.array(row)
        self._cmp_and_set(new_tiles)


    def _set_cols(self, cols):
        new_tiles = self._create_empty_tiles()
        for i,col in enumerate(cols):
            new_tiles[:, i] = np.array(col)
        self._cmp_and_set(new_tiles)            


    def _cmp_and_set(self, tiles):
        old_tiles = self._tiles
        self._tiles = tiles
        self._is_changed = not((old_tiles == tiles).all())


    def _reverse(self, arr):
        return arr[::-1]


    def _merge(self, arr):
        # Remove all 0 from array.  Done if nothing is left
        lst = [e for e in arr if e != 0]
        if len(lst) == 0:
            return arr
        # Run recursively merge
        res = self._merge_helper(lst[:1], lst[1:], False)
        # Create array
        narr = arr.size
        if len(res) < narr:
            res.extend([0] * (narr - len(res)))
        new_array = np.array(res, int)
        return new_array


    def _merge_helper(self, before, after, is_merged):
        # Recursive function to merge tiles in after into before
        if len(after) == 0:
            return before
        b = before[-1]
        a = after[0]
        after = after[1:]
        if not(is_merged) and b == a:
            before = before[:-1] + [a+b]
            return self._merge_helper(before, after, True)
        else:
            before = before + [a]
            return self._merge_helper(before, after, False)


    def __str__(self):
        return str(self._tiles)



        
            
class TwentyFortyEightTest(unittest.TestCase):

    def setUp(self):
        game = TwentyFortyEight(5, 4, lambda *args: 0)
        game.set_tile(0, 1, 2)
        game.set_tile(0, 2, 2)
        game.set_tile(1, 1, 2)
        game.set_tile(1, 2, 4)
        game.set_tile(1, 3, 4)
        game.set_tile(2, 2, 8)
        game.set_tile(2, 3, 16)
        self.game = game

    def test_setup(self):
        game = self.game
        self.assertEqual(2, game.get_tile(0, 1))
        self.assertEqual(4, game.get_tile(1, 2))
        self.assertEqual(8, game.get_tile(2, 2))
        self.assertEqual(4, game.get_width())
        self.assertEqual(5, game.get_height())

    def test_reset(self):
        game = self.game
        game.reset()
        self.assertEqual(0, game.get_tile(0, 1))
        self.assertEqual(0, game.get_tile(2, 3))

    def test_move_up(self):
        game = self.game
        game.move(TwentyFortyEight.UP)
        self.assertEqual(4, game.get_tile(0, 1))
        self.assertEqual(4, game.get_tile(0, 3))
        self.assertEqual(16, game.get_tile(1, 3))
        self.assertEqual(0, game.get_tile(1, 1))

    def test_move_down(self):
        game = self.game
        game.move(TwentyFortyEight.DOWN)
        self.assertEqual(4, game.get_tile(4, 1))
        self.assertEqual(8, game.get_tile(4, 2))
        self.assertEqual(4, game.get_tile(3, 2))
        self.assertEqual(2, game.get_tile(2, 2))
        self.assertEqual(0, game.get_tile(1, 2))

    def test_move_left(self):
        game = self.game
        game.move(TwentyFortyEight.LEFT)
        self.assertEqual(4, game.get_tile(0, 0))
        self.assertEqual(2, game.get_tile(1, 0))
        self.assertEqual(8, game.get_tile(1, 1))
        self.assertEqual(0, game.get_tile(0, 1))

    def test_move_right(self):
        game = self.game
        game.move(TwentyFortyEight.RIGHT)
        self.assertEqual(4, game.get_tile(0, 3))
        self.assertEqual(0, game.get_tile(0, 2))
        self.assertEqual(0, game.get_tile(0, 1))
        self.assertEqual(0, game.get_tile(0, 2))
        self.assertEqual(8, game.get_tile(1, 3))
        self.assertEqual(16, game.get_tile(2, 3))

    def test_is_won(self):
        game = self.game
        self.assertFalse(game.is_won())
        game.set_tile(3, 3, 2048)
        self.assertTrue(game.is_won())

    def test_is_lost(self):
        game = self.game
        game.reset()
        self.assertFalse(game.is_lost())
        tile = 1
        # Fill out all tiles with different number
        for row in range(game.get_height()):
            for col in range(game.get_width()):
                game.set_tile(row, col, tile)
                tile += 1
        self.assertTrue(game.is_lost())
        # Make 2 tiles on same row same number
        orig_tile = game.get_tile(1, 2)
        game.set_tile(1, 2, game.get_tile(1, 1))
        self.assertFalse(game.is_lost())
        game.set_tile(1, 2, orig_tile)
        self.assertTrue(game.is_lost())
        # Make 2 tiles on same column same number
        orig_tile = game.get_tile(3, 2)
        game.set_tile(3, 2, game.get_tile(4, 2))
        self.assertFalse(game.is_lost())
        

if __name__ == '__main__':
    unittest.main()
        
    
        

        
    
