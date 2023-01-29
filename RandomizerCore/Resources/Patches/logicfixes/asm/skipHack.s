.thumb
ldr	r3,poin
loop:
ldr	r2,[r3]
add	r3,#4
cmp	r2,#0
beq	Vanilla
cmp	r2,r0
bne	loop

New:
ldr	r3,=#0x80169CA
mov	lr,r3
.short	0xF800
ldr	r2,=#0xFC00
and	r0,r2
b	End

Vanilla:
ldr	r3,=#0x80169CA
mov	lr,r3
.short	0xF800

End:
mov	r1,r0
ldr	r0,=#0xFFFF
ldr	r3,=#0x807D9C5
bx	r3

.align
.ltorg
poin:
