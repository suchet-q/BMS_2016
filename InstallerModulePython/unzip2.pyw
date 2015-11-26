import zipfile
import subprocess
from Tkinter import Tk
from tkFileDialog import askopenfilename
from tkFileDialog import askdirectory
import os
import sys

if __name__ == '__main__':

	try:
		myFormats = [
		('ModulFile','*.mod'),
		]
		Tk().withdraw()
		if len(sys.argv) == 1:
			filename = askopenfilename(filetypes = myFormats, title = 'Choose a file to extract')
			zfile = zipfile.ZipFile(filename)
		else:
			zfile = zipfile.ZipFile(sys.argv[1])
			
		path = os.environ.get('MODULE_PATH') # change with the path
		print path
		
		if path != None :
			zfile.extractall(path)
		else :
			Tk().withdraw()
			directory = askdirectory(initialdir = "/", title = 'Please select a directory')
			zfile.extractall(directory)
	except:
		import sys
		print sys.exc_info()[0]
		import traceback
		print traceback.format_exc()
		print "Exception appear. Please try again.\n Press 'Enter' to continue ..." 
		raw_input()