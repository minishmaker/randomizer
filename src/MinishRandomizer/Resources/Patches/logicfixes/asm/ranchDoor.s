.thumb
push	{lr}
mov	r0,#0x1D
ldr	r3,=#0x807C654
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	nodoor

ldr	r0,poin
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800
b	end

nodoor:
ldr	r0,=#0x80F2DF4
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800

end:
pop	{pc}
.align
.ltorg
poin:
