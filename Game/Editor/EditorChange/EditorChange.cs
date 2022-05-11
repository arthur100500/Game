namespace Platformer.Common.EditorChange
{
	public abstract class EditorChange
	{
		public abstract void Undo(Editor editor);

		public abstract void Redo(Editor editor);
	}
}