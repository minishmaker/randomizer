.thumb
@check our custom nut flag
ldr	r0,=#0x2002EA4	@base offset
mov	r1,#0		@flag id to check
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	End

@call some routine
ldr	r3,=#0x805E208
mov	lr,r3
.short	0xF800

@and return
End:
ldr	r3,=#0x8094941
bx	r3
