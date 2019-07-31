.thumb
@check our custom nut flag
ldr	r0,=#0x2002EA4	@base offset
mov	r1,#2		@flag id to check
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	End

@spawn the mushroom
ldr	r0,=#0x80F94D4
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800

End:
pop	{pc}
